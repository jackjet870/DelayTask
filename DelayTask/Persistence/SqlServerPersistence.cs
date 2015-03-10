using DelayTask.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using EntityFramework.Future;

namespace DelayTask.Persistence
{
    /// <summary>
    /// SqlServer任务数据持久化实现
    /// </summary>
    public class SqlServerPersistence : ITaskPersistence
    {
        /// <summary>
        /// 更新保存的历史数据结构，使之和当前最新的Model相对应     
        /// </summary>
        public void MigrateDatasToLatestVersion()
        {
            Database.SetInitializer<DatabaseContext>(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        /// <summary>
        /// 获取全部任务
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskBase> GetTaskList()
        {
            using (var dbContext = new DatabaseContext())
            {
                var sqlTasks = dbContext.SqlTask.ToList().Cast<TaskBase>();
                var httpTasks = dbContext.HttpTask.ToList().Cast<TaskBase>();
                return sqlTasks.Union(httpTasks);
            }
        }

        /// <summary>
        /// 设置任务
        /// </summary>
        /// <typeparam name="T">任务类型</typeparam>
        /// <param name="task">任务</param>
        /// <returns></returns>
        private bool SetTask<T>(T task) where T : TaskBase
        {
            using (var dbContext = new DatabaseContext())
            {
                var dbSet = dbContext.Set<T>();
                if (dbSet.Any(item => item.ID == task.ID) == false)
                {
                    dbSet.Add(task);
                }
                else
                {
                    var oldEntity = dbSet.Find(task.ID);
                    dbContext.Entry(oldEntity).CurrentValues.SetValues(task);
                }

                try
                {
                    dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 设置任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns></returns>
        public bool SetTask(TaskBase task)
        {
            if (task == null)
            {
                return false;
            }

            var type = task.GetType();
            if (type == typeof(HttpTask))
            {
                return this.SetTask<HttpTask>(task as HttpTask);
            }
            else if (type == typeof(SqlTask))
            {
                return this.SetTask<SqlTask>(task as SqlTask);
            }

            return false;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool RemoveTask(TaskBase task)
        {
            if (task == null)
            {
                return false;
            }

            using (var dbContext = new DatabaseContext())
            {
                var type = task.GetType();
                if (type == typeof(HttpTask))
                {
                    dbContext.HttpTask.Where(item => item.ID == task.ID).Delete();
                }
                else if (type == typeof(SqlTask))
                {
                    dbContext.SqlTask.Where(item => item.ID == task.ID).Delete();
                }
                return true;
            }
        }

        /// <summary>
        /// 释放资源 
        /// </summary>
        public void Dispose()
        {
        }
    }
}
