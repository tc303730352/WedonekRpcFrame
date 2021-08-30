using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 自动任务
        /// </summary>
        public class AutoTask
        {
                /// <summary>
                /// 所属集群Id
                /// </summary>
                [NumValidate("rpc.mer.id.error", 1)]
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务名
                /// </summary>
                [NullValidate("rpc.task.name.null")]
                [LenValidate("rpc.task.name.len", 2, 50)]
                public string TaskName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务类型
                /// </summary>
                [EnumValidate("rpc.taskType.error", typeof(TaskType))]
                public TaskType TaskType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务间隔
                /// </summary>
                [NumValidate("rpc.task.timespan.error", 1, int.MaxValue)]
                public int TaskTimeSpan
                {
                        get;
                        set;
                }

                /// <summary>
                /// 任务排序
                /// </summary>
                [NumValidate("rpc.task.priority.error", 0, int.MaxValue)]
                public int TaskPriority
                {
                        get;
                        set;
                }
                /// <summary>
                /// 发送类型错误
                /// </summary>
                [EnumValidate("rpc.taskType.error", typeof(TaskSendType))]
                public TaskSendType SendType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 发送参数
                /// </summary>
                [NullValidate("rpc.task.sendParam.null")]
                public TaskSendParam SendParam
                {
                        get;
                        set;
                }

        }
}
