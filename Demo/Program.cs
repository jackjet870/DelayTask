using DelayTaskLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DelayTaskClient("127.0.0.1", 12306);
            var task = new SqlDelayTask { ID = Guid.NewGuid(), Name = "sql", ExecuteTime = DateTime.Now.AddSeconds(20) };
            var add = client.SetSqlTask(task).Result;
            var page = client.SqlTaskToPage(0, 10, null, null, null).Result;

            Console.ReadLine();
        }
    }
}
