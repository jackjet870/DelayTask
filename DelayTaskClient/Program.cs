using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskClient
{
    class Program
    {
        static DelayTaskLib.DelayTaskClient client = new DelayTaskLib.DelayTaskClient("127.0.0.1", 12346);

        static void Main(string[] args)
        {
            var ad = client.SetHttpTask(DelayTaskLib.DelayTaskConfig.NewUnloopTaskConfig(Guid.NewGuid(), 10, null, null), "abc", "").Result;
            var page = client.GeTaskPage(0, 10, string.Empty).Result;
        }
    }
}
