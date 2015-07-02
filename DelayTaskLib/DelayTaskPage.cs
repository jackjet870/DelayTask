using System;
using System.Collections.Generic;
using System.Linq;

namespace DelayTaskLib
{
    /// <summary>
    /// 表示延时任务的分页信息   
    /// </summary>   
    [Serializable]
    public class DelayTaskPage
    {
        /// <summary>
        /// 页面索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页面数据
        /// </summary>
        public List<DelayTask> EntityArray { get; set; }

        /// <summary>
        /// 任务分页信息
        /// </summary>
        public DelayTaskPage()
        {
            this.EntityArray = new List<DelayTask>();
        }
    }
}
