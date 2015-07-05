using DelayTaskServer.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using System.Linq.Expressions;

namespace DelayTaskServer.Sheduler
{
    /// <summary>
    /// Db操作
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// 加载待执行的任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LoadTasks<T>() where T : DelayTask
        {
            using (var db = new DatabaseContext())
            {
                return db.Set<T>()
                    .Where(item => item.LoopInterval > 0 || (item.SuccessCount + item.FailureCount) == 0)
                    .ToList();
            }
        }

        /// <summary>
        /// 添加执行结果记录
        /// </summary>
        /// <param name="result">执行结果</param>
        /// <returns></returns>
        public static bool AddExecResult<T>(DelayTaskExecResult result) where T : DelayTask, new()
        {
            using (var db = new DatabaseContext())
            {
                if (result.Success == true)
                {
                    db.Set<T>().Where(item => item.ID == result.DelayTaskID).Update(item => new T { SuccessCount = item.SuccessCount + 1 });
                }
                else
                {
                    db.Set<T>().Where(item => item.ID == result.DelayTaskID).Update(item => new T { FailureCount = item.FailureCount + 1 });
                }
                db.DelayTaskExecResult.Add(result);
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 添加或更新任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="forUpdate"></param>
        /// <returns></returns>
        public static bool SetTask<T>(T task, bool forUpdate) where T : DelayTask
        {
            using (var db = new DatabaseContext())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (forUpdate == true)
                    {
                        db.Set<T>().Where(item => item.ID == task.ID).Delete();
                        db.DelayTaskExecResult.Where(item => item.DelayTaskID == task.ID).Delete();
                    }

                    try
                    {
                        db.Set<T>().Add(task);
                        db.SaveChanges();
                        tran.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }


        /// <summary>
        /// 删除任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public static bool RemoveTask<T>(Guid id) where T : DelayTask
        {
            using (var db = new DatabaseContext())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.Set<T>().Where(item => item.ID == id).Delete();
                    db.DelayTaskExecResult.Where(item => item.DelayTaskID == id).Delete();
                    tran.Commit();
                    return true;
                }
            }
        }


        /// <summary>
        /// 设置任务的执行时间
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="executeTime">执行时间</param>
        public static bool SetExecuteTime<T>(Guid id, DateTime executeTime) where T : DelayTask, new()
        {
            using (var db = new DatabaseContext())
            {
                return db.Set<T>().Where(item => item.ID == id).Update(item => new T { ExecuteTime = executeTime }) > 0;
            }
        }


        /// <summary>
        /// 获取延时任务分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>     
        /// <param name="where">条件</param>    
        /// <param name="orderBy">排序字符串</param>
        /// <returns></returns>
        public static PageInfo<T> DelayTaskToPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> where, string orderBy) where T : DelayTask
        {
            using (var db = new DatabaseContext())
            {
                var query = db.Set<T>().Where(where);
                var totalCount = query.Where(where).Count();
                var model = query.Where(where).OrderBy(orderBy).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return new PageInfo<T>
                {
                    EntityArray = model,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };
            }
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
            using (var db = new DatabaseContext())
            {
                var query = db.DelayTaskExecResult.Where(item => item.DelayTaskID == taskId);
                var totalCount = query.Count();
                var model = query.OrderByDescending(item => item.ExecutingTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return new PageInfo<DelayTaskExecResult>
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
