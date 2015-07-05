using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DelayTaskLib
{
    /// <summary>
    /// 延时任务抽象类  
    /// </summary>
    [Serializable]
    [DebuggerDisplay("ExecuteTime = {ExecuteTime}")]
    public abstract class DelayTask
    {
        /// <summary>
        /// 获取或设置任务的唯一ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 获取任务执行的时间        
        /// </summary>       
        public DateTime ExecuteTime { get; set; }

        /// <summary>
        /// 获取或设置任务名称
        /// 50字符以内
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置任务描述
        /// 200字符以内
        /// </summary>      
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置循环执行的间隔秒数 
        /// 0表示不循环执行
        /// </summary>
        public int LoopInterval { get; set; }

        /// <summary>
        /// 执行成功次数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 执行失败次数
        /// </summary>
        public int FailureCount { get; set; }
    }
}
