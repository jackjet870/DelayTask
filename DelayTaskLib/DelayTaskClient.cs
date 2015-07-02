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
        {
            this.ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(hostOrAddress).Last(), port);
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
        /// <param name="taskConfig">任务配置</param>      
        /// <param name="connectingString">连接字符串</param>       
        /// <param name="sql">SQL语句</param>     
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>     
        public Task<bool> SetSqlTask(DelayTaskConfig taskConfig, string connectingString, string sql)
        {
            return this.Client.InvokeApi<bool>("SetSqlTask", taskConfig, connectingString, sql);
        }

        /// <summary>
        /// 添加或设置Http任务
        /// </summary>            
        /// <param name="taskConfig">任务配置</param>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>  
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>     
        public Task<bool> SetHttpTask(DelayTaskConfig taskConfig, string url, string param)
        {
            return this.Client.InvokeApi<bool>("SetHttpTask", taskConfig, url, param);
        }

        /// <summary>
        /// 添加Http任务
        /// </summary>
        /// <param name="taskConfig">任务配置</param>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>
        public Task<bool> SetHttpTask(DelayTaskConfig taskConfig, string url, object param)
        {
            var paramterString = string.Empty;
            if (param != null)
            {
                var items = param.GetType().GetProperties().Select(item => string.Format("{0}={1}", item.Name, item.GetValue(param, null))).ToArray();
                paramterString = string.Join("&", items);
            }
            return this.SetHttpTask(taskConfig, url, paramterString);
        }


        /// <summary>
        /// 删除任务
        /// </summary>            
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>       
        public Task<bool> RemoveTask(Guid id)
        {
            return this.Client.InvokeApi<bool>("RemoveTask", id);
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>            
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>  
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>     
        public Task<bool> AddTaskDelaySeconds(Guid id, int delaySeconds)
        {
            return this.Client.InvokeApi<bool>("AddTaskDelaySeconds", id, delaySeconds);
        }


        /// <summary>
        /// 获取失败的任务分页
        /// </summary>            
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="sourceId">原始任务的id(Empty表示所有sourceId)</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>        
        public Task<DelayTaskPage> GetFailureTaskPage(int pageIndex, int pageSize, Guid sourceId, string taskType)
        {
            return this.Client.InvokeApi<DelayTaskPage>("GetFailureTaskPage", pageIndex, pageSize, sourceId, taskType);
        }


        /// <summary>
        /// 执行一个失败的任务
        /// 执行成功则自动从失败列表中移除
        /// </summary>            
        /// <param name="id">任务id</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>       
        public Task<bool> ExecuteFailureTask(Guid id)
        {
            return this.Client.InvokeApi<bool>("ExecuteFailureTask", id);
        }

        /// <summary>
        /// 获取待运行的任务分页
        /// </summary>      
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <exception cref="SocketException"></exception>
        /// <returns></returns>      
        public Task<DelayTaskPage> GeTaskPage(int pageIndex, int pageSize, string taskType)
        {
            return this.Client.InvokeApi<DelayTaskPage>("GeTaskPage", pageIndex, pageSize, taskType);
        }
    }
}
