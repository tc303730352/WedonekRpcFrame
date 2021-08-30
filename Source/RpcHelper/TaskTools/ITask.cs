using System;

namespace RpcHelper.TaskTools
{
        public enum TaskType
        {
                定时任务 = 0,
                间隔任务 = 1,
                定时间隔任务 = 2
        }
        public interface ITask : IDisposable
        {
                /// <summary>
                /// 任务ID
                /// </summary>
                string TaskId { set; get; }

                /// <summary>
                /// 任务优先级
                /// </summary>
                int TaskPriority { get; }

                /// <summary>
                /// 任务名
                /// </summary>
                string TaskName { get; }
                /// <summary>
                /// 任务类型
                /// </summary>
                TaskType TaskType { get; }
                /// <summary>
                /// 执行间隔
                /// </summary>
                TimeSpan ExecInterval { get; }

                /// <summary>
                /// 是否只执行一次
                /// </summary>
                bool IsOnce { get; }

                /// <summary>
                /// 执行任务
                /// </summary>
                void ExecuteTask();
        }
}
