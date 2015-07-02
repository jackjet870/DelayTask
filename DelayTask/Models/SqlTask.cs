using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayTask.Models
{
    /// <summary>
    /// Sql任务
    /// </summary>
    [Serializable]
    [DebuggerDisplay("SqlCommand = {SqlCommand}")]
    public class SqlTask : TaskBase
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectingString { get; set; }

        /// <summary>
        /// SQL语句
        /// </summary>
        public string SqlCommand { get; set; }

        /// <summary>
        /// Sql任务
        /// </summary>
        /// <param name="taskConfig">任务配置</param>   
        /// <param name="connectingString">连接字符串</param>
        /// <param name="sql">SQL语句</param>
        public SqlTask(TaskBaseConfig taskConfig, string connectingString, string sql)
            : base(taskConfig)
        {
            this.ConnectingString = connectingString;
            this.SqlCommand = sql;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            try
            {
                using (var connet = new SqlConnection(this.ConnectingString))
                {
                    connet.Open();

                    using (var tran = connet.BeginTransaction())
                    {
                        using (var cmd = new SqlCommand(this.SqlCommand, connet, tran))
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                                tran.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                LastErrors.SetLastError(this.ID, ex);
                                tran.Rollback();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastErrors.SetLastError(this.ID, ex);
            }
            return false;
        }       
    }
}
