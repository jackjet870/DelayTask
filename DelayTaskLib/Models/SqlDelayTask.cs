using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DelayTaskLib
{
    /// <summary>
    /// 表示SQL任务
    /// </summary>
    [Serializable]    
    public class SqlDelayTask : DelayTask
    {
        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        public string ConnectingString { get; set; }

        /// <summary>
        /// 获取或设置SQL语句
        /// </summary>
        public string SqlCommand { get; set; }      
    }
}
