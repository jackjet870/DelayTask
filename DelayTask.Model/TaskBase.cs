using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayTask.Model
{
    /// <summary>
    /// 任务基础类
    /// 要求所有任务从此对象派生
    /// </summary>
    [Serializable]
    public class TaskBase
    {
        /// <summary>
        /// 获取或设置任务的唯一ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 如果任务失败
        /// 则SourceId为原始任务的ID
        /// </summary>
        public Guid SourceId { get; set; }

        /// <summary>
        /// 获取或设置任务执行时间
        /// </summary>
        public DateTime ExecuteTime { get; set; }

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
        /// 获取或设置当前失败后尝试执行的次数
        /// </summary>
        public int TryTime { get; set; }

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
        /// 获取或设置是否已执行过一次
        /// </summary>
        public bool HasExecuted { get; set; }

        /// <summary>
        /// 获取或设置是否有相关的失败任务
        /// </summary>
        public bool HasFailureTask { get; set; }

        /// <summary>
        /// 任务基础类
        /// </summary>
        public TaskBase()
        {
            this.HasExecuted = false;
        }

        /// <summary>
        /// 任务基础类
        /// </summary>
        /// <param name="taskConfig">任务配置</param>     
        public TaskBase(TaskBaseConfig taskConfig)
            : this()
        {
            this.ID = taskConfig.ID;
            this.ExecuteTime = DateTime.Now.AddSeconds(taskConfig.DelaySeconds);
            this.MaxTryTime = taskConfig.MaxTryTime;
            this.TryInterval = taskConfig.TryInterval;
            this.TaskType = taskConfig.TaskType;
            this.Description = taskConfig.Description;
            this.Loop = taskConfig.Loop;
            this.LoopInterval = taskConfig.LoopInterval;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public virtual bool Execute()
        {
            return true;
        }

        /// <summary>
        /// 获取是否可以立即运行
        /// </summary>
        /// <returns></returns>
        public bool CanExcuteNow()
        {
            return this.ExecuteTime <= DateTime.Now;
        }

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <returns></returns>
        public Task<bool> ExecuteAsync()
        {
            return Task.Factory.StartNew(() => this.Execute(), TaskCreationOptions.None);
        }

        /// <summary>
        /// 格式化时间撮
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public string FormatTimeSpan(TimeSpan timespan)
        {
            var times = new[]
            { 
                new{ value = timespan.Days, text="天"},   
                new{ value = timespan.Hours,text="小时"},
                new{ value = timespan.Minutes,text="分钟"},
                new{ value = timespan.Seconds,text="秒"},
            };

            var format = times.Where(item => item.value > 0);
            if (format.Count() == 0)
            {
                format = new[] { times.Last() };
            }
            return string.Join("", format.Select(item => string.Format("{0}{1}", item.value, item.text)));
        }

        /// <summary>
        /// 转换为失败的任务
        /// </summary>
        /// <returns></returns>
        public virtual TaskBase ToFailureTask()
        {
            var task = this.MemberwiseClone() as TaskBase;
            task.SourceId = task.ID;
            task.ID = Guid.NewGuid();
            task.HasFailureTask = false;
            return task;
        }
    }
}
