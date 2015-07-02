using DelayTask.Sheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace DelayTask
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
                c.Service<TaskTcpServer>();
                c.RunAsLocalSystem();
                c.SetServiceName("DelayTask");
                c.SetDisplayName("延时任务执行服务");
                c.SetDescription("提供延时和定时任务执行的服务");
            });
        }
    }
}
