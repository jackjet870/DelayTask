using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskLib
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum DelayTaskState
    {
        /// <summary>
        /// 全部状态
        /// 0
        /// </summary>
        ALL,

        /// <summary>
        /// 活动的任务
        /// 1
        /// </summary>
        Active,

        /// <summary>
        /// 不再活动的任务
        /// 2
        /// </summary>
        NoActive
    }
}
