using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace DelayTaskServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            HostFactory.Run(c =>
            {
                c.Service<TcpServer>();
                c.RunAsLocalSystem();
                c.SetServiceName("DelayTaskServer");
                c.SetDisplayName("延时任务执行服务");
                c.SetDescription("提供延时和定时任务执行的服务");
            });
        }
    }
}
