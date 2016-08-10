using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DelayTaskServer.Tasks
{
    /// <summary>
    /// 任务基础类
    /// 要求所有任务从此对象派生
    /// </summary>
    [Serializable]
    [DebuggerDisplay("ExecuteTime = {ExecuteTime}")]
    public abstract class DelayTask
    {
        /// <summary>
        /// 获取或设置任务的唯一ID
        /// </summary>       
        public Guid ID { get; set; }

        /// <summary>
        /// 获取或设置任务执行时间
        /// </summary>       
        public DateTime ExecuteTime { get; set; }

        /// <summary>
        /// 获取或设置任务名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置任务描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置循环执行的间隔秒数 
        /// 0表示不循环执行
        /// </summary>
        public int LoopInterval { get; set; }

        /// <summary>
        /// 获取或设置执行成功次数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 获取或设置执行失败次数
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// 获取或设置是否正在执行中的标记
        /// </summary>      
        [NotMapped]
        [ScriptIgnore]
        public bool IsExecuting { get; set; }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> Execute();

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
        public async Task<DelayTaskExecResult> ExecuteAsync()
        {
            var result = new DelayTaskExecResult
            {
                ID = Guid.NewGuid(),
                DelayTaskID = this.ID,
                DelayTaskType = this.GetType().Name,
                ExecutingTime = DateTime.Now
            };

            try
            {
                result.Success = await this.Execute();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex.Message;
            }
            finally
            {
                result.ExecutedTime = DateTime.Now;
            }
            return result;
        }
    }
}
