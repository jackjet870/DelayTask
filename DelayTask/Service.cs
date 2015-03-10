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
        private TaskTcpService service;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.service = new TaskTcpService();
            this.service.StartListen(12346);
        }

        protected override void OnStop()
        {
            this.service.Dispose();
        }
    }
}
