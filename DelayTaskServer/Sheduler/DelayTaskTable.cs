using DelayTaskServer.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using System.Threading.Tasks;

namespace DelayTaskServer.Sheduler
{
    /// <summary>
    /// 延时任务列表
    /// </summary>
    public abstract class DelayTaskTable<T> where T : DelayTask, new()
    {
        /// <summary>
        /// 任务字典
        /// </summary>
        private ConcurrentDictionary<Guid, T> taskTable = new ConcurrentDictionary<Guid, T>();

        /// <summary>
        /// 延时任务列表
        /// </summary>
        public DelayTaskTable()
        {
            using (var db = new DatabaseContext())
            {
                var tasks = db.Set<T>().Include("FailureTasks").Where(item => item.IsExecuted == false).ToList();
                foreach (var task in tasks)
                {
                    this.taskTable.TryAdd(task.ID, task);
                }
            }
        }

        /// <summary>
        /// 检测任务的执行
        /// </summary>
        public void CheckForExcute()
        {
            var tasks = this.taskTable.Values.Where(item => item.CanExcuteNow() && item.IsExecuted == false);
            foreach (var task in tasks)
            {
                task.IsExecuted = true;
                task.ExecuteAsync().ContinueWith(t => this.OnTaskExecuted(task, t.Result));
            }
        }

        /// <summary>
        /// 任务执行后
        /// </summary>
        /// <param name="task"></param>
        /// <param name="result"></param>
        private void OnTaskExecuted(T task, ExecResult result)
        {
            if (task.LoopInterval > 0)
            {
                this.AddDelay(task.ID, task.LoopInterval);
                task.IsExecuted = false;
            }
            else
            {
                this.Remove(task.ID, false);
                using (var db = new DatabaseContext())
                {
                    db.Set<T>().Where(item => item.ID == task.ID).Update(item => new T { IsExecuted = true });
                }
            }

            if (result.State == false)
            {
                this.OnCreateFailureTask(task, result.Message);
            }
        }


        /// <summary>
        /// 创建失败的任务执行记录
        /// </summary>
        /// <param name="task">任务</param>     
        /// <param name="reason">失败原因</param>
        public abstract void OnCreateFailureTask(T task, string reason);

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        public T Get(Guid id)
        {
            T task;
            this.taskTable.TryGetValue(id, out task);
            return task;
        }

        /// <summary>
        /// 设置一个任务
        /// </summary>
        /// <param name="task">任务</param>
        public bool Set(T task)
        {
            if (task == null)
            {
                return false;
            }

            var exists = this.taskTable.ContainsKey(task.ID);
            this.taskTable.AddOrUpdate(task.ID, task, (k, v) => task);

            using (var db = new DatabaseContext())
            {
                if (exists == false)
                {
                    db.Set<T>().Add(task);
                }
                else
                {
                    var oldTask = db.Set<T>().Find(task.ID);
                    db.Entry(oldTask).CurrentValues.SetValues(task);
                }

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除任务       
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="dbSync">是否同步数据库</param>
        public bool Remove(Guid id, bool dbSync)
        {
            T task;
            if (this.taskTable.TryRemove(id, out task) == false)
            {
                return false;
            }

            if (dbSync == false)
            {
                return true;
            }

            using (var db = new DatabaseContext())
            {
                return db.Set<T>().Where(item => item.ID == id).Delete() > 0;
            }
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="seconds">延长秒数</param>
        public bool AddDelay(Guid id, int seconds)
        {
            var task = this.Get(id);
            if (task == null)
            {
                return false;
            }

            task.ExecuteTime = task.ExecuteTime.AddSeconds(seconds);
            using (var db = new DatabaseContext())
            {
                return db.Set<T>().Where(item => item.ID == id).Update(item => new T { ExecuteTime = task.ExecuteTime }) > 0;
            }
        }

        /// <summary>
        /// 获取待运行的任务分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="isExecuted">是否已执行</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        public DelayTaskPage<T> ToPage(int pageIndex, int pageSize, bool? isExecuted, string name, string description)
        {
            var where = Where.True<T>();
            if (isExecuted.HasValue)
            {
                where = where.And(item => item.IsExecuted == isExecuted);
            }
            if (string.IsNullOrEmpty(name) == false)
            {
                where = where.And(item => item.Name.Contains(name));
            }
            if (string.IsNullOrEmpty(description) == false)
            {
                where = where.And(item => item.Description.Contains(description));
            }

            using (var db = new DatabaseContext())
            {
                var totalCount = db.Set<T>().Where(where).Count();
                var model = db.Set<T>().Include("FailureTasks").Where(where).OrderBy(item => item.ExecuteTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return new DelayTaskPage<T>
                {
                    EntityArray = model,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };
            }
        }
    }
}
