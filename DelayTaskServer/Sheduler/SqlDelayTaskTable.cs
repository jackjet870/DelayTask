using DelayTaskServer.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer.Sheduler
{
    /// <summary>
    /// Sql延时任务列表
    /// </summary>
    public class SqlDelayTaskTable : DelayTaskTable<SqlDelayTask>
    {
        /// <summary>
        /// 创建失败的任务执行记录
        /// </summary>
        /// <param name="task">任务</param>     
        /// <param name="reason">失败原因</param>
        public override void OnCreateFailureTask(SqlDelayTask task, string reason)
        {
            var failureTask = new FailureSqlTask
            {
                ID = Guid.NewGuid(),
                SqlDelayTaskID = task.ID,
                ExecuteTime = task.ExecuteTime,
                FailureReason = reason
            };

            task.FailureTasks.Add(failureTask);
            using (var db = new DatabaseContext())
            {
                db.FailureSqlTask.Add(failureTask);
                db.SaveChanges();
            }
        }
    }
}
