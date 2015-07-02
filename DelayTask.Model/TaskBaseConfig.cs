using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTask.Model
{
    /// <summary>
    /// 任务的配置信息
    /// </summary>
    [Serializable]
    public class TaskBaseConfig
    {
        /// <summary>
        /// 获取或设置任务的唯一ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 获取或设置任务执行延时执行时间
        /// </summary>
        public int DelaySeconds { get; set; }

        /// <summary>
        /// 获取或设置任务类型
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// 获取或设置任务描述
        /// </summary>
        public string Description { get; set; }



        /// <summary>
        /// 获取或设置是否轮回执行
        /// 当为true时，MaxTryTime只为能1
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// 获取或设置轮回执行的间隔秒数 
        /// </summary>
        public int LoopInterval { get; set; }



        /// <summary>
        /// 获取或设置任务失败后尝试执行的次数 
        /// 当Loop为true时，此参数不起作用
        /// </summary>
        public int MaxTryTime { get; set; }

        /// <summary>
        /// 获取或设置失败尝试的时间间隔（秒）
        /// 当Loop为false且MaxTryTime大于0时才生效
        /// </summary>
        public int TryInterval { get; set; }


        /// <summary>
        /// 获取一个周期运行的任务配置信息
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延时执行的秒数</param>
        /// <param name="taskType">类型标识</param>
        /// <param name="description">描述</param>
        /// <param name="loopInterval">运行时间间隔(秒)</param>
        /// <returns></returns>
        public static TaskBaseConfig NewLoopTaskConfig(Guid id, int delaySeconds, string taskType, string description, int loopInterval)
        {
            return new TaskBaseConfig
            {
                ID = id,
                DelaySeconds = delaySeconds,
                TaskType = taskType,
                Description = description,

                Loop = true,
                LoopInterval = loopInterval
            };
        }

        /// <summary>
        ///  获取一个单一运行的任务配置信息
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延时执行的秒数</param>
        /// <param name="taskType">类型标识</param>
        /// <param name="description">描述</param>
        /// <param name="maxTryTime">失败后最大尝试运行次数</param>
        /// <param name="tryInterval">尝试时间间隔(秒)</param>
        /// <returns></returns>
        public static TaskBaseConfig NewUnloopTaskConfig(Guid id, int delaySeconds, string taskType, string description, int maxTryTime = 0, int tryInterval = 0)
        {
            return new TaskBaseConfig
            {
                ID = id,
                DelaySeconds = delaySeconds,
                TaskType = taskType,
                Description = description,

                MaxTryTime = maxTryTime,
                TryInterval = tryInterval
            };
        }
    }
}
