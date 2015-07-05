using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace DelayTaskServer.Tasks
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

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public override void Execute()
        {
            using (var client = new WebClient())
            {
                var bytes = new byte[0];
                if (string.IsNullOrEmpty(this.Param) == false)
                {
                    bytes = Encoding.UTF8.GetBytes(this.Param);
                }
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.UploadData(this.URL, "post", bytes);
            }
        }
    }
}
