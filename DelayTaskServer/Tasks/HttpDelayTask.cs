using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 表示Http任务
    /// </summary>
    [Serializable]
    public class HttpDelayTask : DelayTask
    {
        /// <summary>
        /// http客户端
        /// </summary>
        private static readonly HttpClient httpClient = new HttpClient();


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
        public override async Task<bool> Execute()
        {
            var content = this.Param == null ? new byte[0] : Encoding.UTF8.GetBytes(this.Param);
            var httpContent = new ByteArrayContent(content);
            httpContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            var response = await httpClient.PostAsync(this.URL, httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}
