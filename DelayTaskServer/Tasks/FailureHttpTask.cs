using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 失败的Http任务
    /// </summary>
    [Serializable]
    public class FailureHttpTask
    {
        /// <summary>
        /// 获取或设置ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 获取或设置关联的Http任务ID
        /// </summary>
        public Guid HttpDelayTaskID { get; set; }

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
