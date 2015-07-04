using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 失败的SQL任务
    /// </summary>
    [Serializable]
    public class FailureSqlTask
    {
        /// <summary>
        /// 获取或设置ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 获取或设置关联的SQL任务ID
        /// </summary>
        public Guid SqlDelayTaskID { get; set; }

        /// <summary>
        /// 获取任务执行的时间        
        /// </summary>       
        public DateTime ExecuteTime { get; set; }

        /// <summary>
        /// 获取或设置失败原因
        /// </summary>
        public string FailureReason { get; set; }
    }
}
