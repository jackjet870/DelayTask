using DelayTask.Model;
using NetworkSocket;
using NetworkSocket.Fast;
using NetworkSocket.Fast.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DelayTask.Client
{
    /// <summary>
    /// 客户端实现
    /// </summary>
    public class DelayTaskClient : FastTcpClientBase
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object syncRoot = new object();
        /// <summary>
        /// 实例
        /// </summary>
        private static DelayTaskClient instance;

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        public static DelayTaskClient Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null || instance.IsDisposed)
                    {
                        instance = new DelayTaskClient();
                    }
                    if (instance.IsConnected == false)
                    {
                        instance.Connect();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// 连接超时5秒
        /// </summary>
        private readonly int connectTimeOut = 5 * 1000;

        /// <summary>
        /// 获取请求超时30秒
        /// </summary>
        public static readonly int TimeOut = 30 * 1000;

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            var ip = IPAddress.Parse(ConfigurationManager.AppSettings["DelayTaskService"]);
            var connect = this.Connect(ip, 12346);
            return connect.Wait(this.connectTimeOut) && connect.Result;
        }


        /// <summary>
        /// 获取任务的最近错误信息
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <returns></returns>
        [Service(Implements.Remote, 100)]
        public Task<string> GetTaskLastError(Guid id)
        {
            return this.InvokeRemote<string>(100, id);
        }

        /// <summary>
        /// 添加或设置Sql任务
        /// </summary>            
        /// <param name="taskConfig">任务配置</param>      
        /// <param name="connectingString">连接字符串</param>
        /// <param name="sql">SQL语句</param>
        [Service(Implements.Remote, 101)]
        public Task<bool> SetSqlTask(TaskBaseConfig taskConfig, string connectingString, string sql)
        {
            return this.InvokeRemote<bool>(101, taskConfig, connectingString, sql);
        }

        /// <summary>
        /// 添加或设置Http任务
        /// </summary>            
        /// <param name="taskConfig">任务配置</param>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        [Service(Implements.Remote, 102)]
        public Task<bool> SetHttpTask(TaskBaseConfig taskConfig, string url, string param)
        {
            return this.InvokeRemote<bool>(102, taskConfig, url, param);
        }

        /// <summary>
        /// 添加Http任务
        /// </summary>
        /// <param name="taskConfig">任务配置</param>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns></returns>
        public Task<bool> SetHttpTask(TaskBaseConfig taskConfig, string url, object param)
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
        /// <returns></returns>
        [Service(Implements.Remote, 103)]
        public Task<bool> RemoveTask(Guid id)
        {
            return this.InvokeRemote<bool>(103, id);
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>            
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>
        [Service(Implements.Remote, 104)]
        public Task<bool> AddTaskDelaySeconds(Guid id, int delaySeconds)
        {
            return this.InvokeRemote<bool>(104, id, delaySeconds);
        }


        /// <summary>
        /// 获取失败的任务分页
        /// </summary>            
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="sourceId">原始任务的id(Empty表示所有sourceId)</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <returns></returns>
        [Service(Implements.Remote, 105)]
        public Task<TaskBasePage> GetFailureTaskPage(int pageIndex, int pageSize, Guid sourceId, string taskType)
        {
            return this.InvokeRemote<TaskBasePage>(105, pageIndex, pageSize, sourceId, taskType);
        }


        /// <summary>
        /// 执行一个失败的任务
        /// 执行成功则自动从失败列表中移除
        /// </summary>            
        /// <param name="id">任务id</param>
        /// <returns></returns>
        [Service(Implements.Remote, 106)]
        public Task<bool> ExecuteFailureTask(Guid id)
        {
            return this.InvokeRemote<bool>(106, id);
        }

        /// <summary>
        /// 获取待运行的任务分页
        /// </summary>      
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <returns></returns>
        [Service(Implements.Remote, 107)]
        public Task<TaskBasePage> GeTaskPage(int pageIndex, int pageSize, string taskType)
        {
            return this.InvokeRemote<TaskBasePage>(107, pageIndex, pageSize, taskType);
        }
    }
}
