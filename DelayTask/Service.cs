using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DelayTask
{
    public partial class Service : ServiceBase
    {
        private TaskTcpServer tcpServer;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.tcpServer = new TaskTcpServer();
            this.tcpServer.StartListen();
        }

        protected override void OnStop()
        {
            this.tcpServer.Dispose();
        }
    }
}
