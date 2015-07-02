using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTask.Models
{
    /// <summary>
    /// 任务的配置信息
    /// </summary>
    [Serializable]
    public class TaskBaseConfig
    {
        /// <summary>
        /// 获取或设置任务的唯一ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 获取或设置任务执行延时执行时间
        /// </summary>
        public int DelaySeconds { get; set; }

        /// <summary>
        /// 获取或设置任务类型
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// 获取或设置任务描述
        /// </summary>
        public string Description { get; set; }



        /// <summary>
        /// 获取或设置是否轮回执行
        /// 当为true时，MaxTryTime只为能1
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// 获取或设置轮回执行的间隔秒数 
        /// </summary>
        public int LoopInterval { get; set; }



        /// <summary>
        /// 获取或设置任务失败后尝试执行的次数 
        /// 当Loop为true时，此参数不起作用
        /// </summary>
        public int MaxTryTime { get; set; }

        /// <summary>
        /// 获取或设置失败尝试的时间间隔（秒）
        /// 当Loop为false且MaxTryTime大于0时才生效
        /// </summary>
        public int TryInterval { get; set; }         
    }
}
