using DelayTaskServer.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace DelayTaskServer.Sheduler
{
    /// <summary>
    /// 延时任务列表
    /// </summary>
    public class DelayTaskTable<T> where T : DelayTask, new()
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
            var tasks = DatabaseHelper.LoadTasks<T>();
            foreach (var task in tasks)
            {
                this.taskTable.TryAdd(task.ID, task);
            }
        }

        /// <summary>
        /// 检测任务的执行
        /// </summary>
        public void CheckForExcute()
        {
            var tasks = this.taskTable.Values.Where(item => item.CanExcuteNow() && item.IsExecuting == false);
            foreach (var task in tasks)
            {
                task.IsExecuting = true;
                task.ExecuteAsync().ContinueWith(t => this.OnTaskExecuted(task, t.Result));
            }
        }

        /// <summary>
        /// 任务执行后
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="result">结果</param>
        private void OnTaskExecuted(T task, DelayTaskExecResult result)
        {
            if (task.LoopInterval > 0)
            {
                this.AddDelay(task.ID, task.LoopInterval);
                task.IsExecuting = false;
            }
            else
            {
                this.Remove(task.ID, false);
                task.IsExecuting = false;
            }

            DatabaseHelper.AddExecResult<T>(result);
        }

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

            var forUpdate = this.taskTable.ContainsKey(task.ID);
            var state = DatabaseHelper.SetTask<T>(task, forUpdate);
            if (state == true)
            {
                this.taskTable.AddOrUpdate(task.ID, task, (k, v) => task);
            }
            return state;
        }

        /// <summary>
        /// 删除任务       
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="dbSync">是否同步数据库</param>
        public bool Remove(Guid id, bool dbSync)
        {
            if (dbSync == true)
            {
                DatabaseHelper.RemoveTask<T>(id);
            }
            T task;
            return this.taskTable.TryRemove(id, out task);
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
            return DatabaseHelper.SetExecuteTime<T>(task.ID, task.ExecuteTime);
        }
    }
}
