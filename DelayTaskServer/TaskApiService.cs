using DelayTaskServer.Tasks;
using NetworkSocket;
using NetworkSocket.Core;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer
{
    /// <summary>
    /// 延时任务服务
    /// </summary>
    public class TaskApiService : FastApiService
    {
        /// <summary>
        /// 获取活动状态SQL任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        [Api]
        public SqlDelayTask GetSqlTask(Guid id)
        {
            return TaskSheduler.SqlTaskTable.Get(id);
        }

        /// <summary>
        /// 添加或设置SQL任务
        /// </summary>       
        /// <param name="task">任务</param>  
        /// <returns></returns>      
        [Api]
        public bool SetSqlTask(SqlDelayTask task)
        {
            return TaskSheduler.SqlTaskTable.Set(task);
        }

        /// <summary>
        /// 删除SQL任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>        
        [Api]
        public bool RemoveSqlTask(Guid id)
        {
            return TaskSheduler.SqlTaskTable.Remove(id, true);
        }

        /// <summary>
        /// 继续延长SQL任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>       
        [Api]
        public bool AddSqlTaskDelay(Guid id, int delaySeconds)
        {
            return TaskSheduler.SqlTaskTable.AddDelay(id, delaySeconds);
        }

        /// <summary>
        /// 获取待运行的SQL任务分页
        /// </summary>  
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>   
        /// <param name="state">状态</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="orderBy">排序字符串</param>     
        /// <returns></returns>
        [Api]
        public PageInfo<SqlDelayTask> SqlTaskToPage(int pageIndex, int pageSize, DelayTaskState state, string keyword, string orderBy)
        {
            return TaskSheduler.SqlTaskToPage(pageIndex, pageSize, state, keyword, orderBy);
        }




        /// <summary>
        /// 获取活动状态Http任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        [Api]
        public HttpDelayTask GetHttpTask(Guid id)
        {
            return TaskSheduler.HttpTaskTable.Get(id);
        }

        /// <summary>
        /// 添加或设置Http任务
        /// </summary>      
        /// <param name="task">任务</param>  
        /// <returns></returns>     
        [Api]
        public bool SetHttpTask(HttpDelayTask task)
        {
            return TaskSheduler.HttpTaskTable.Set(task);
        }

        /// <summary>
        /// 删除Http任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>        
        [Api]
        public bool RemoveHttpTask(Guid id)
        {
            return TaskSheduler.HttpTaskTable.Remove(id, true);
        }

        /// <summary>
        /// 继续延长Http任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>       
        [Api]
        public bool AddHttpTaskDelay(Guid id, int delaySeconds)
        {
            return TaskSheduler.HttpTaskTable.AddDelay(id, delaySeconds);
        }

        /// <summary>
        /// 获取待运行的Http任务分页
        /// </summary>  
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>   
        /// <param name="state">状态</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="orderBy">排序字符串</param>     
        /// <returns></returns>
        [Api]
        public PageInfo<HttpDelayTask> HttpTaskToPage(int pageIndex, int pageSize, DelayTaskState state, string keyword, string orderBy)
        {
            return TaskSheduler.HttpTaskToPage(pageIndex, pageSize, state, keyword, orderBy);
        }



        /// <summary>
        /// 获取任务的执行记录
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        [Api]
        public PageInfo<DelayTaskExecResult> TaskExecResultToPage(Guid taskId, int pageIndex, int pageSize)
        {
            return TaskSheduler.TaskExecResultToPage(taskId, pageIndex, pageSize);
        }
    }
}
