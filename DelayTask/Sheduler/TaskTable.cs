using DelayTask.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTask.Sheduler
{
    /// <summary>
    /// 延时任务列表
    /// </summary>
    public sealed class TaskTable : IDisposable
    {
        /// <summary>
        /// 任务数据持久化
        /// </summary>
        private ITaskPersistence taskPersistence;

        /// <summary>
        /// 任务字典
        /// </summary>
        private ConcurrentDictionary<Guid, TaskBase> taskList;

        /// <summary>
        /// 获取一个任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public TaskBase this[Guid id]
        {
            get
            {
                TaskBase task;
                this.taskList.TryGetValue(id, out task);
                return task;
            }
        }

        /// <summary>
        /// 延时任务列表
        /// </summary>
        public TaskTable()
        {
            var persistenceType = Type.GetType(System.Configuration.ConfigurationManager.AppSettings["Persistence"]);
            this.taskPersistence = (ITaskPersistence)Activator.CreateInstance(persistenceType);
            this.taskList = new ConcurrentDictionary<Guid, TaskBase>();
        }

        /// <summary>
        /// 在try块执行
        /// </summary>
        /// <param name="action"></param>
        private bool TryInvoke(Action action)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 从持久层加载所有任务
        /// </summary>
        public void LoadTasksFromPersistence()
        {
            this.TryInvoke(() =>
            {
                this.taskPersistence.MigrateDatasToLatestVersion();
                var tasks = this.taskPersistence.GetTaskList();
                foreach (var item in tasks)
                {
                    this.taskList.TryAdd(item.ID, item);
                }
            });
        }

        /// <summary>
        /// 获取当前待执行的任务
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskBase> GetTasks()
        {
            return this.taskList.Values.Where(item => item.SourceId == Guid.Empty);
        }

        /// <summary>
        /// 获取已失败的任务
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskBase> GetFailureTasks()
        {
            return this.taskList.Values.Where(item => item.SourceId != Guid.Empty);
        }

        /// <summary>
        /// 设置一个任务
        /// </summary>
        /// <param name="task">任务</param>
        public bool SetTask(TaskBase task)
        {
            if (task != null)
            {
                this.taskList.AddOrUpdate(task.ID, task, (k, v) => task);
                this.TryInvoke(() => this.taskPersistence.SetTask(task));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id">任务id</param>
        public bool RemoveTask(Guid id)
        {
            TaskBase task;
            var result = this.taskList.TryRemove(id, out task);

            if (result == true)
            {
                LastErrors.Remove(id);
                this.TryInvoke(() => this.taskPersistence.RemoveTask(task));
            }
            return result;
        }

        /// <summary>
        /// 获取失败的任务分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="sourceId">原始任务的id(Empty表示所有sourceId)</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <returns></returns>
        public TaskBasePage GetFailureTaskPage(int pageIndex, int pageSize, Guid sourceId, string taskType)
        {
            var where = Where.True<TaskBase>().And(item => item.SourceId != Guid.Empty);
            if (string.IsNullOrWhiteSpace(taskType) == false)
            {
                where = where.And(item => item.TaskType == taskType);
            }
            if (sourceId != Guid.Empty)
            {
                where = where.And(item => item.SourceId == sourceId);
            }

            var query = this.taskList.Values.Where(where.Compile()).OrderBy(item => item.ExecuteTime);
            var model = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            var totalCount = query.Count();

            return new TaskBasePage
            {
                EntityArray = model,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// 获取待运行的任务分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <returns></returns>
        public TaskBasePage GetTaskPage(int pageIndex, int pageSize, string taskType)
        {
            Func<TaskBase, bool> where = (item) => item.SourceId == Guid.Empty;
            if (string.IsNullOrWhiteSpace(taskType) == false)
            {
                where = (item) => item.TaskType == taskType;
            }

            var query = this.taskList.Values.Where(where).OrderBy(item => item.ExecuteTime);
            var model = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            var totalCount = query.Count();

            return new TaskBasePage
            {
                EntityArray = model,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>
        public bool AddTaskDelaySeconds(Guid id, int delaySeconds)
        {
            var task = this[id];
            if (task != null)
            {
                task.ExecuteTime = task.ExecuteTime.AddSeconds(delaySeconds);
                this.SetTask(task);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行一个失败的任务
        /// 执行成功则自动从失败列表中移除
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public bool ExecuteFailureTask(Guid id)
        {
            var task = this[id];
            if (task == null)
            {
                LastErrors.SetLastError(id, new Exception("找不到相关的任务"));
                return false;
            }

            if (task.SourceId == Guid.Empty)
            {
                LastErrors.SetLastError(id, new Exception("任务不是已失败的任务"));
                return false;
            }

            if (task.ExecuteAsync().Result == true)
            {
                this.RemoveTask(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 释放资源 
        /// </summary>
        public void Dispose()
        {
            this.taskPersistence.Dispose();
        }
    }
}
