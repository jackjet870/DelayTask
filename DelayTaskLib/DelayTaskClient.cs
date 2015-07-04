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
        private FastTcpClient Client
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
        /// 获取任务的最近错误信息
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>      
        public Task<string> GetTaskLastError(Guid id)
        {
            return this.Client.InvokeApi<string>("GetTaskLastError", id);
        }

        /// <summary>
        /// 添加或设置Sql任务
        /// </summary>       
        /// <param name="task">任务</param>  
        /// <returns></returns>      
        [Api]
        public Task<bool> SetSqlTask(SqlDelayTask task)
        {
            return this.Client.InvokeApi<bool>("SetSqlTask", task);
        }

        /// <summary>
        /// 删除任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>        
        [Api]
        public Task<bool> RemoveSqlTask(Guid id)
        {
            return this.Client.InvokeApi<bool>("RemoveSqlTask", id);
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>       
        [Api]
        public Task<bool> AddSqlTaskDelay(Guid id, int delaySeconds)
        {
            return this.Client.InvokeApi<bool>("AddSqlTaskDelay", id, delaySeconds);
        }


        /// <summary>
        /// 获取分页
        /// </summary>  
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="isExecuted">是否已执行</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        [Api]
        public Task<DelayTaskPage<SqlDelayTask>> SqlTaskToPage(int pageIndex, int pageSize, bool? isExecuted, string name, string description)
        {
            return this.Client.InvokeApi<DelayTaskPage<SqlDelayTask>>("SqlTaskToPage", pageIndex, pageSize, isExecuted, name, description);
        }


        /// <summary>
        /// 添加或设置Http任务
        /// </summary>      
        /// <param name="task">任务</param>  
        /// <returns></returns>     
        [Api]
        public Task<bool> SetHttpTask(HttpDelayTask task)
        {
            return this.Client.InvokeApi<bool>("SetHttpTask", task);
        }


        /// <summary>
        /// 删除任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>        
        [Api]
        public Task<bool> RemoveHttpTask(Guid id)
        {
            return this.Client.InvokeApi<bool>("RemoveHttpTask", id);
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>       
        [Api]
        public Task<bool> AddHttpTaskDelay(Guid id, int delaySeconds)
        {
            return this.Client.InvokeApi<bool>("AddHttpTaskDelay", id, delaySeconds);
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
        public Task<DelayTaskPage<HttpDelayTask>> HttpTaskToPage(int pageIndex, int pageSize, bool? isExecuted, string name, string description)
        {
            return this.Client.InvokeApi<DelayTaskPage<HttpDelayTask>>("HttpTaskToPage", pageIndex, pageSize, isExecuted, name, description);
        }
    }
}
