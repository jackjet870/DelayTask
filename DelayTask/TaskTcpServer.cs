using NetworkSocket;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using DelayTask.Model;
using System.Configuration;

namespace DelayTask
{
    /// <summary>
    /// 网络任务服务
    /// </summary>
    public class TaskTcpServer : FastTcpServerBase
    {
        /// <summary>
        /// 网络任务服务
        /// </summary>
        public TaskTcpServer()
        {
            this.BindService<TaskTcpService>();
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        public void StartListen()
        {
            var port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            this.StartListen(port);
        }
    }
}
