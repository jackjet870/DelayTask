using DelayTask.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DelayTask
{
    /// <summary>
    /// 任务调度器
    /// 线程安全类型
    /// 此类不能继承
    /// </summary>
    public sealed class TaskSheduler
    {
        /// <summary>
        /// 获取任务列表
        /// </summary>
        public TaskList TaskList { get; private set; }


        /// <summary>
        /// 任务调度器
        /// </summary>
        public TaskSheduler()
        {
            this.TaskList = new TaskList();
            this.TaskList.LoadTasksFromPersistence();
        }


        /// <summary>
        /// 开始调度任务
        /// </summary>
        public void StartSheduler()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    this.Sheduler();
                    Thread.Sleep(100);
                }
            });
        }


        /// <summary>
        /// 调度任务
        /// </summary>       
        private void Sheduler()
        {
            var tasks = this.TaskList.GetTasks().Where(item => item.CanExcuteNow());
            foreach (var task in tasks)
            {
                if (task.Loop == true)
                {
                    var failureTask = task.ToFailureTask();
                    task.ExecuteTime = task.ExecuteTime.AddSeconds(task.LoopInterval);
                    failureTask.ExecuteAsync().ContinueWith(t => this.LoopTaskResult(t.Result, task, failureTask));
                }
                else
                {
                    this.TaskList.RemoveTask(task.ID);
                    if (task.HasExecuted == false)
                    {
                        task.HasExecuted = true;
                    }
                    else
                    {
                        task.TryTime = task.TryTime + 1;
                    }
                    task.ExecuteAsync().ContinueWith(t => this.UnLoopTaskResult(t.Result, task));
                }
            }
        }

        /// <summary>
        /// 处理Loop任务结束
        /// </summary>
        /// <param name="result">执行结果</param>    
        /// <param name="task">原始任务</param>
        /// <param name="failureTask">失败任务</param>
        private void LoopTaskResult(bool result, TaskBase task, TaskBase failureTask)
        {
            if (result == false)
            {
                if (task.HasFailureTask == false)
                {
                    task.HasFailureTask = true;
                    this.TaskList.SetTask(task);
                }
                this.TaskList.SetTask(failureTask);
            }
        }

        /// <summary>
        /// 处理任务的执行结束
        /// </summary>
        /// <param name="result">执行结果</param>
        /// <param name="task">任务</param>
        private void UnLoopTaskResult(bool result, TaskBase task)
        {
            // 执行成功则不用鸟它了
            if (result == true)
            {
                return;
            }

            if (task.TryTime < task.MaxTryTime)
            {
                task.ExecuteTime = task.ExecuteTime.AddSeconds(task.TryInterval);
                this.TaskList.SetTask(task);
            }
            else
            {
                var failureTask = task.ToFailureTask();
                this.TaskList.SetTask(failureTask);
            }
        }       
    }
}
