using DelayTaskServer.Tasks;
using NetworkSocket;
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
    public class TaskService : FastApiService
    { 
        /// <summary>
        /// 添加或设置Sql任务
        /// </summary>       
        /// <param name="task">任务</param>  
        /// <returns></returns>      
        [Api]
        public bool SetSqlTask(SqlDelayTask task)
        {
            return TaskSheduler.SqlTaskTable.Set(task);
        }

        /// <summary>
        /// 删除任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>        
        [Api]
        public bool RemoveSqlTask(Guid id)
        {
            return TaskSheduler.SqlTaskTable.Remove(id, true);
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>       
        [Api]
        public bool AddSqlTaskDelay(Guid id, int delaySeconds)
        {
            return TaskSheduler.SqlTaskTable.AddDelay(id, delaySeconds);
        }

        /// <summary>
        /// 分页操作
        /// </summary>  
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="isExecuted">是否已执行</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        [Api]
        public DelayTaskPage<SqlDelayTask> SqlTaskToPage(int pageIndex, int pageSize, bool? isExecuted, string name, string description)
        {
            return TaskSheduler.SqlTaskTable.ToPage(pageIndex, pageSize, isExecuted, name, description);
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
        /// 删除任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>        
        [Api]
        public bool RemoveHttpTask(Guid id)
        {
            return TaskSheduler.HttpTaskTable.Remove(id, true);
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>       
        [Api]
        public bool AddHttpTaskDelay(Guid id, int delaySeconds)
        {
            return TaskSheduler.HttpTaskTable.AddDelay(id, delaySeconds);
        }

        /// <summary>
        /// 分页操作
        /// </summary>  
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="isExecuted">是否已执行</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        [Api]
        public DelayTaskPage<HttpDelayTask> HttpTaskToPage(int pageIndex, int pageSize, bool? isExecuted, string name, string description)
        {
            return TaskSheduler.HttpTaskTable.ToPage(pageIndex, pageSize, isExecuted, name, description);
        }
    }
}
