using RpcHelper.TaskTools;
using RpcHelper.Validate;
namespace AutoTaskService.Model
{
        internal class RemoteTask
        {
                /// <summary>
                /// 集群ID
                /// </summary>
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务名
                /// </summary>
                [NullValidate("rpc.task.name.null")]
                public string TaskName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务类型
                /// </summary>
                [EnumValidate("rpc.task.type.error", typeof(TaskType))]
                public TaskType TaskType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务间隔
                /// </summary>
                [NumValidate("rpc.task.type.error", 0, int.MaxValue)]
                public int TaskTimeSpan
                {
                        get;
                        set;
                }
                /// <summary>
                /// 任务排序
                /// </summary>
                public int TaskPriority
                {
                        get;
                        set;
                }
                /// <summary>
                /// 发送类型错误
                /// </summary>
                [EnumValidate("rpc.task.send.Type.error", typeof(TaskSendType))]
                public TaskSendType SendType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 发送参数
                /// </summary>
                [EntrustValidate("_CheckTask")]
                public SendParam SendParam
                {
                        get;
                        set;
                }
                public int VerNum { get; internal set; }

                private static bool _CheckTask(RemoteTask obj, out string error)
                {
                        if (obj.TaskType != TaskType.定时任务 && obj.TaskTimeSpan == 0)
                        {
                                error = "rpc.task.send.timespan.null";
                                return false;
                        }
                        else if (obj.SendParam == null)
                        {
                                error = "rpc.task.send.param.null";
                                return false;
                        }
                        else if (obj.SendType == TaskSendType.URI && obj.SendParam.HttpConfig == null)
                        {
                                error = "rpc.task.send.param.http.config.null";
                                return false;
                        }
                        else if (obj.SendType == TaskSendType.指令 && obj.SendParam.RpcConfig == null)
                        {
                                error = "rpc.task.send.param.rpc.config.null";
                                return false;
                        }
                        else if (obj.SendType == TaskSendType.广播 && obj.SendParam.BroadcastConfig == null)
                        {
                                error = "rpc.task.send.param.broadcast.config.null";
                                return false;
                        }
                        error = null;
                        return true;
                }
        }
}
