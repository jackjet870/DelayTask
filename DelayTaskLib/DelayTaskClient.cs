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
        /// 远程端
        /// </summary>
        private IPEndPoint ipEndPoint;

        /// <summary>
        /// 客户端对象
        /// </summary>
        private FastTcpClient client = new FastTcpClient();

        /// <summary>
        /// 获取自动连接的客户端对象
        /// </summary>
        private FastTcpClient ConnClient
        {
            get
            {
                if (this.client.IsConnected == false)
                {
                    this.client.Connect(this.ipEndPoint).Wait();
                }
                return this.client;
            }
        }

        /// <summary>
        /// 延时执行任务客户端
        /// </summary>
        /// <param name="remoteEndPoint">远程端地址</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelayTaskClient(IPEndPoint remoteEndPoint)
        {
            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException();
            }
            this.ipEndPoint = remoteEndPoint;
            this.client.Serializer = new JavaScriptSerializer();
        }

        /// <summary>
        /// 延时执行任务客户端
        /// </summary>
        /// <param name="hostOrAddress">ip或域名</param>
        /// <param name="port">远程端口</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="SocketException"></exception>
        public DelayTaskClient(string hostOrAddress, int port)
            : this(new IPEndPoint(Dns.GetHostAddresses(hostOrAddress).Last(), port))
        {
        }



        /// <summary>
        /// 获取活动状态SQL任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>     
        public Task<SqlDelayTask> GetSqlTask(Guid id)
        {
            return this.ConnClient.InvokeApi<SqlDelayTask>("GetSqlTask", id);
        }

        /// <summary>
        /// 添加或设置SQL任务
        /// </summary>       
        /// <param name="task">任务</param> 
        /// <exception cref="SocketException"></exception>
        /// <returns></returns> 
        public Task<bool> SetSqlTask(SqlDelayTask task)
        {
            return this.ConnClient.InvokeApi<bool>("SetSqlTask", task);
        }

        /// <summary>
        /// 删除SQL任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>  
        public Task<bool> RemoveSqlTask(Guid id)
        {
            return this.ConnClient.InvokeApi<bool>("RemoveSqlTask", id);
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
            return this.ConnClient.InvokeApi<bool>("AddSqlTaskDelay", id, delaySeconds);
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
            return this.ConnClient.InvokeApi<PageInfo<SqlDelayTask>>("SqlTaskToPage", pageIndex, pageSize, state, keyword, orderBy);
        }




        /// <summary>
        /// 获取活动状态Http任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>   
        public Task<HttpDelayTask> GetHttpTask(Guid id)
        {
            return this.ConnClient.InvokeApi<HttpDelayTask>("GetHttpTask", id);
        }

        /// <summary>
        /// 添加或设置Http任务
        /// </summary>      
        /// <param name="task">任务</param>  
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>   
        public Task<bool> SetHttpTask(HttpDelayTask task)
        {
            return this.ConnClient.InvokeApi<bool>("SetHttpTask", task);
        }


        /// <summary>
        /// 删除Http任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>    
        public Task<bool> RemoveHttpTask(Guid id)
        {
            return this.ConnClient.InvokeApi<bool>("RemoveHttpTask", id);
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
            return this.ConnClient.InvokeApi<bool>("AddHttpTaskDelay", id, delaySeconds);
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
            return this.ConnClient.InvokeApi<PageInfo<SqlDelayTask>>("HttpTaskToPage", pageIndex, pageSize, state, keyword, orderBy);
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
            return this.ConnClient.InvokeApi<PageInfo<DelayTaskExecResult>>("TaskExecResultToPage", taskId, pageIndex, pageSize);
        }
    }
}
