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
        public static SqlDelayTaskTable SqlTaskTable { get; private set; }

        /// <summary>
        /// 获取HTTP任务列表
        /// </summary>
        public static HttpDelayTaskTable HttpTaskTable { get; private set; }

        /// <summary>
        /// 任务调度器
        /// </summary>
        static TaskSheduler()
        {
            Database.SetInitializer<DatabaseContext>(new CreateDatabaseIfNotExists<DatabaseContext>());
            Database.SetInitializer<DatabaseContext>(new MigrateDatabaseToLatestVersion<DatabaseContext, DelayTaskServer.Sheduler.Migrations.Configuration>());

            TaskSheduler.SqlTaskTable = new SqlDelayTaskTable();
            TaskSheduler.HttpTaskTable = new HttpDelayTaskTable();

            LoopWorker.AddWork(() => SqlTaskTable.CheckForExcute());
            LoopWorker.AddWork(() => HttpTaskTable.CheckForExcute());
        }
    }
}
