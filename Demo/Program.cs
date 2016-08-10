using DelayTaskLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DelayTaskClient(new IPEndPoint(IPAddress.Loopback, 12306));

            // 20 秒后删除SqlDelayTask表内的所有数据
            var task = new SqlDelayTask
            {
                ID = Guid.NewGuid(),
                Name = "SQL任务1",
                Description = "这是一个测试SQL任务",
                ExecuteTime = DateTime.Now.AddSeconds(20),
                ConnectingString = "Data Source=.;Initial Catalog=DelayTask; Persist Security Info=True;User ID=sa;Password=123456",
                SqlCommand = "DELETE FROM [SqlDelayTask]"
            };

            
            var state = client.SetSqlTask(task).Result;
            Console.WriteLine("设置任务{0}", state ? "成功" : "失败");
            Console.ReadLine();
        }
    }
}
