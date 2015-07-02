using DelayTask.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DelayTask.Sheduler
{
    /// <summary>
    /// 任务调度器
    /// 线程安全类型
    /// 此类不能继承
    /// </summary>
    public sealed class TaskSheduler
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static TaskSheduler instance;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly static object syncRoot = new object();

        /// <summary>
        /// 获取任务列表操作实例
        /// </summary>
        public static TaskTable TaskTable
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new TaskSheduler();
                    }
                    return instance.taskList;
                }
            }
        }

        /// <summary>
        /// 任务列表
        /// </summary>
        private TaskTable taskList = new TaskTable();

        /// <summary>
        /// 任务调度器
        /// </summary>
        private TaskSheduler()
        {
            this.taskList.LoadTasksFromPersistence();

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
            var tasks = this.taskList.GetTasks().Where(item => item.CanExcuteNow());
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
                    this.taskList.RemoveTask(task.ID);
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
                    this.taskList.SetTask(task);
                }
                this.taskList.SetTask(failureTask);
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
                this.taskList.SetTask(task);
            }
            else
            {
                var failureTask = task.ToFailureTask();
                this.taskList.SetTask(failureTask);
            }
        }
    }
}
