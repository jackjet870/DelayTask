using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace DelayTaskLib
{
    /// <summary>
    /// 表示Http任务
    /// </summary>
    [Serializable]
    public class HttpDelayTask : DelayTask
    {
        /// <summary>
        /// 获取或设置请求的URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 获取或设置请求的参数
        /// </summary>
        public string Param { get; set; }
    }
}
