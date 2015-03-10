using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace DelayTask.Model
{
    /// <summary>
    /// 表示Http任务
    /// </summary>
    [Serializable]
    [DebuggerDisplay("URL = {URL}")]
    public class HttpTask : TaskBase
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
        /// 表示Http任务
        /// </summary>
        public HttpTask()
        {
        }

        /// <summary>
        /// Http任务
        /// </summary>
        /// <param name="taskConfig">任务配置</param>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        public HttpTask(TaskBaseConfig taskConfig, string url, string param)
            : base(taskConfig)
        {
            this.URL = url;
            this.Param = param;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            using (var client = new WebClient())
            {
                try
                {
                    var bytes = new byte[0];
                    if (string.IsNullOrEmpty(this.Param) == false)
                    {
                        bytes = Encoding.UTF8.GetBytes(this.Param);
                    }
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.UploadData(this.URL, "post", bytes);
                    return true;
                }
                catch (Exception ex)
                {
                    LastErrors.SetLastError(this.ID, ex);
                    return false;
                }
            }
        }
    }
}
