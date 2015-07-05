using DelayTaskServer.Sheduler;
using DelayTaskServer.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DelayTaskServer
{
    /// <summary>
    /// 表示任务调度器
    /// </summary>
    public static class TaskSheduler
    {
        /// <summary>
        /// 获取SQL任务列表
        /// </summary>
        public static DelayTaskTable<SqlDelayTask> SqlTaskTable { get; private set; }

        /// <summary>
        /// 获取HTTP任务列表
        /// </summary>
        public static DelayTaskTable<HttpDelayTask> HttpTaskTable { get; private set; }

        /// <summary>
        /// 任务调度器
        /// </summary>
        static TaskSheduler()
        {
            Database.SetInitializer<DatabaseContext>(new CreateDatabaseIfNotExists<DatabaseContext>());
            Database.SetInitializer<DatabaseContext>(new MigrateDatabaseToLatestVersion<DatabaseContext, DelayTaskServer.Sheduler.Migrations.Configuration>());

            TaskSheduler.SqlTaskTable = new DelayTaskTable<SqlDelayTask>();
            TaskSheduler.HttpTaskTable = new DelayTaskTable<HttpDelayTask>();

            LoopWorker.AddWork(() => SqlTaskTable.CheckForExcute());
            LoopWorker.AddWork(() => HttpTaskTable.CheckForExcute());
        }


        /// <summary>
        /// 获取任务的执行记录
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public static PageInfo<DelayTaskExecResult> TaskExecResultToPage(Guid taskId, int pageIndex, int pageSize)
        {
            return DatabaseHelper.TaskExecResultToPage(taskId, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取待运行的SQL任务分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>    
        /// <param name="state">状态</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        public static PageInfo<SqlDelayTask> SqlTaskToPage(int pageIndex, int pageSize, DelayTaskState state, string name, string description)
        {
            var where = Where.True<SqlDelayTask>();

            if (state == DelayTaskState.Active)
            {
                where = where.And(item => item.LoopInterval > 0 || (item.SuccessCount + item.FailureCount) == 0);
            }
            else if (state == DelayTaskState.NoActive)
            {
                where = where.And(item => item.LoopInterval <= 0 && (item.SuccessCount + item.FailureCount) > 0);
            }

            if (string.IsNullOrEmpty(name) == false)
            {
                where = where.And(item => item.Name.Contains(name));
            }
            if (string.IsNullOrEmpty(description) == false)
            {
                where = where.And(item => item.Description.Contains(description));
            }

            return DatabaseHelper.DelayTaskToPage<SqlDelayTask>(pageIndex, pageSize, where);
        }


        /// <summary>
        /// 获取待运行的Http任务分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>      
        /// <param name="state">状态</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        public static PageInfo<HttpDelayTask> HttpTaskToPage(int pageIndex, int pageSize, DelayTaskState state, string name, string description)
        {
            var where = Where.True<HttpDelayTask>();

            if (state == DelayTaskState.Active)
            {
                where = where.And(item => item.LoopInterval > 0 || (item.SuccessCount + item.FailureCount) == 0);
            }
            else if (state == DelayTaskState.NoActive)
            {
                where = where.And(item => item.LoopInterval <= 0 && (item.SuccessCount + item.FailureCount) > 0);
            }

            if (string.IsNullOrEmpty(name) == false)
            {
                where = where.And(item => item.Name.Contains(name));
            }
            if (string.IsNullOrEmpty(description) == false)
            {
                where = where.And(item => item.Description.Contains(description));
            }
            return DatabaseHelper.DelayTaskToPage<HttpDelayTask>(pageIndex, pageSize, where);
        }
    }
}
