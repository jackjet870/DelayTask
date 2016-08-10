using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 表示Sql任务
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

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> Execute()
        {
            using (var connet = new SqlConnection(this.ConnectingString))
            {
                connet.Open();
                using (var tran = connet.BeginTransaction())
                {
                    using (var cmd = new SqlCommand(this.SqlCommand, connet, tran))
                    {
                        await cmd.ExecuteNonQueryAsync();
                        tran.Commit();
                        return true;
                    }
                }
            }
        }
    }
}
