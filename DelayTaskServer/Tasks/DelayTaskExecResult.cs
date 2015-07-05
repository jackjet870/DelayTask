using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 延时任务执行结果
    /// </summary>
    public class DelayTaskExecResult
    {
        /// <summary>
        /// 获取或设置ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 获取或设置关联的任务ID
        /// </summary>
        public Guid DelayTaskID { get; set; }

        /// <summary>
        /// 获取或设置任务类型
        /// </summary>
        [StringLength(30)]
        public string DelayTaskType { get; set; }

        /// <summary>
        /// 获取任务的开始执行时间        
        /// </summary>       
        public DateTime ExecutingTime { get; set; }

        /// <summary>
        /// 获取任务的结束执行时间        
        /// </summary>       
        public DateTime ExecutedTime { get; set; }

        /// <summary>
        /// 获取或设置是否执行成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 获取或设置异常消息
        /// </summary>
        public string Exception { get; set; }
    }
}
