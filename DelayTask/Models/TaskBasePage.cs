using System;
using System.Collections.Generic;
using System.Linq;

namespace DelayTask.Models
{
    /// <summary>
    /// 任务分页信息   
    /// </summary>   
    [Serializable]
    public class TaskBasePage
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
        public List<TaskBase> EntityArray { get; set; }

        /// <summary>
        /// 任务分页信息
        /// </summary>
        public TaskBasePage()
        {
            this.EntityArray = new List<TaskBase>();
        }
    }
}
