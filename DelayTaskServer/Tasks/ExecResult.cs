using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 执行结果
    /// </summary>
    public class ExecResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }
        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; set; }
    }
}
