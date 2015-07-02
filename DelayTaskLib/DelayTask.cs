using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayTaskLib
{
    /// <summary>
    /// 表示延时任务
    /// </summary>
    [Serializable]
    public class DelayTask
    {
        /// <summary>
        /// 获取或设置任务的唯一ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 如果任务失败
        /// 则SourceId为原始任务的ID
        /// </summary>
        public Guid SourceId { get; set; }

        /// <summary>
        /// 获取或设置任务执行时间
        /// </summary>
        public DateTime ExecuteTime { get; set; }

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
        /// 获取或设置当前失败后尝试执行的次数
        /// </summary>
        public int TryTime { get; set; }

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

        /// <summary>
        /// 获取或设置是否已执行过一次
        /// </summary>
        public bool HasExecuted { get; set; }

        /// <summary>
        /// 获取或设置是否有相关的失败任务
        /// </summary>
        public bool HasFailureTask { get; set; }
    }
}
