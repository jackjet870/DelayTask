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
    public class ListenerControl : ServiceControl
    {
        /// <summary>
        /// tcp服务
        /// </summary>
        private TcpListener listener = new TcpListener();


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Start(Topshelf.HostControl hostControl)
        {
            var middleware = new FastMiddleware();
            middleware.Serializer = new JavaScriptSerializer();
            middleware.GlobalFilters.Add(new ExceptionFilter());
            listener.Use(middleware);

            var port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            listener.Start(port);
            return true;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Stop(Topshelf.HostControl hostControl)
        {
            this.listener.Dispose();
            return true;
        }
    }
}
