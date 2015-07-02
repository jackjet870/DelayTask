using DelayTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTask
{
    /// <summary>
    /// 任务数据持久化接口
    /// </summary>
    public interface ITaskPersistence : IDisposable
    {
        /// <summary>
        /// 更新保存的历史数据结构，使之和当前最新的Model相对应     
        /// </summary>
        void MigrateDatasToLatestVersion();

        /// <summary>
        /// 添加或设置任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        bool SetTask(TaskBase task);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns></returns>
        bool RemoveTask(TaskBase task);

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        IEnumerable<TaskBase> GetTaskList();
    }
}
