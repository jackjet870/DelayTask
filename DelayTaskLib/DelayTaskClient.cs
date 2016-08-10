using NetworkSocket;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DelayTaskLib
{
    /// <summary>
    /// 表示延时执行任务客户端
    /// </summary>
    public class DelayTaskClient
    {
        /// <summary>
        /// 客户端对象
        /// </summary>
        private readonly FastTcpClient client = new FastTcpClient();

        /// <summary>
        /// 延时执行任务客户端
        /// </summary>
        /// <param name="remoteEndPoint">远程端地址</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelayTaskClient(EndPoint remoteEndPoint)
        {
            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException();
            }

            this.client.Serializer = new JavaScriptSerializer();
            this.client.TrySetKeepAlive(TimeSpan.FromMinutes(1));
            this.client.AutoReconnect = TimeSpan.FromSeconds(5);
            this.client.Connect(remoteEndPoint);
        }

        /// <summary>
        /// 获取活动状态SQL任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>     
        public Task<SqlDelayTask> GetSqlTask(Guid id)
        {
            return this.client.InvokeApi<SqlDelayTask>("GetSqlTask", id);
        }

        /// <summary>
        /// 添加或设置SQL任务
        /// </summary>       
        /// <param name="task">任务</param> 
        /// <exception cref="SocketException"></exception>
        /// <returns></returns> 
        public Task<bool> SetSqlTask(SqlDelayTask task)
        {
            return this.client.InvokeApi<bool>("SetSqlTask", task);
        }

        /// <summary>
        /// 删除SQL任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>  
        public Task<bool> RemoveSqlTask(Guid id)
        {
            return this.client.InvokeApi<bool>("RemoveSqlTask", id);
        }

        /// <summary>
        /// 继续延长SQL任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>  
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>
        public Task<bool> AddSqlTaskDelay(Guid id, int delaySeconds)
        {
            return this.client.InvokeApi<bool>("AddSqlTaskDelay", id, delaySeconds);
        }


        /// <summary>
        /// 获取SQL任务分页
        /// </summary>  
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>  
        /// <param name="state">任务状态</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="orderBy">排序字符串</param>     
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>      
        public Task<PageInfo<SqlDelayTask>> SqlTaskToPage(int pageIndex, int pageSize, DelayTaskState state, string keyword = null, string orderBy = "ExecuteTime ASC")
        {
            return this.client.InvokeApi<PageInfo<SqlDelayTask>>("SqlTaskToPage", pageIndex, pageSize, state, keyword, orderBy);
        }




        /// <summary>
        /// 获取活动状态Http任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>   
        public Task<HttpDelayTask> GetHttpTask(Guid id)
        {
            return this.client.InvokeApi<HttpDelayTask>("GetHttpTask", id);
        }

        /// <summary>
        /// 添加或设置Http任务
        /// </summary>      
        /// <param name="task">任务</param>  
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>   
        public Task<bool> SetHttpTask(HttpDelayTask task)
        {
            return this.client.InvokeApi<bool>("SetHttpTask", task);
        }


        /// <summary>
        /// 删除Http任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>    
        public Task<bool> RemoveHttpTask(Guid id)
        {
            return this.client.InvokeApi<bool>("RemoveHttpTask", id);
        }

        /// <summary>
        /// 继续延长Http任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>   
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>
        public Task<bool> AddHttpTaskDelay(Guid id, int delaySeconds)
        {
            return this.client.InvokeApi<bool>("AddHttpTaskDelay", id, delaySeconds);
        }

        /// <summary>
        /// 获取Http任务分页
        /// </summary>  
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>      
        /// <param name="state">任务状态</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="orderBy">排序字符串</param>  
        /// <exception cref="SocketException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>       
        public Task<PageInfo<SqlDelayTask>> HttpTaskToPage(int pageIndex, int pageSize, DelayTaskState state, string keyword = null, string orderBy = "ExecuteTime ASC")
        {
            return this.client.InvokeApi<PageInfo<SqlDelayTask>>("HttpTaskToPage", pageIndex, pageSize, state, keyword, orderBy);
        }



        /// <summary>
        /// 获取延时任务的执行记录
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>    
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>
        public Task<PageInfo<DelayTaskExecResult>> TaskExecResultToPage(Guid taskId, int pageIndex, int pageSize)
        {
            return this.client.InvokeApi<PageInfo<DelayTaskExecResult>>("TaskExecResultToPage", taskId, pageIndex, pageSize);
        }
    }
}
