using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum DelayTaskState
    {
        /// <summary>
        /// 全部状态
        /// </summary>
        ALL,
        /// <summary>
        /// 活动的任务
        /// </summary>
        Active,
        /// <summary>
        /// 不再活动的任务
        /// </summary>
        NoActive
    }
}
