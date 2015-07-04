using DelayTaskServer.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer.Sheduler
{
    /// <summary>
    /// Http延时任务列表
    /// </summary>
    public class HttpDelayTaskTable : DelayTaskTable<HttpDelayTask>
    {
        /// <summary>
        /// 创建失败的任务执行记录
        /// </summary>
        /// <param name="task">任务</param>     
        /// <param name="reason">失败原因</param>
        public override void OnCreateFailureTask(HttpDelayTask task, string reason)
        {
            var failureTask = new FailureHttpTask
            {
                ID = Guid.NewGuid(),
                HttpDelayTaskID = task.ID,
                ExecuteTime = task.ExecuteTime,
                FailureReason = reason
            };

            task.FailureTasks.Add(failureTask);
            using (var db = new DatabaseContext())
            {
                db.FailureHttpTask.Add(failureTask);
                db.SaveChanges();
            }
        }
    }
}
