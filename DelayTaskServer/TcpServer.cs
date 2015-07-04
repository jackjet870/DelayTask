using NetworkSocket;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using Topshelf;

namespace DelayTaskServer
{
    /// <summary>
    /// DelayTask的Tcp服务
    /// </summary>
    public class TcpServer : FastTcpServer, ServiceControl
    {
        /// <summary>
        /// 异常非正常的数据请求会话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="exception"></param>
        protected override void OnException(object sender, Exception exception)
        {
            if (exception is ProtocolException)
            {
                var sesssion = sender as FastSession;
                sesssion.Close();
            }
            base.OnException(sender, exception);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(Topshelf.HostControl hostControl)
        {
            var port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            this.Serializer = new JavaScriptSerializer();
            this.BindService<TaskService>();
            this.GlobalFilter.Add(new ExceptionFilter());
            this.StartListen(port);
            return true;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(Topshelf.HostControl hostControl)
        {
            this.Dispose();
            return true;
        }
    }
}
