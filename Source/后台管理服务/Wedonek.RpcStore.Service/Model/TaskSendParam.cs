using RpcHelper.Validate;
namespace Wedonek.RpcStore.Service.Model
{
        public class TaskSendParam
        {
                /// <summary>
                /// Http配置
                /// </summary>
                [EntrustValidate("_CheckSendParam")]
                public HttpParamConfig HttpConfig
                {
                        get;
                        set;
                }

                /// <summary>
                /// Rpc通信配置
                /// </summary>
                public RpcParamConfig RpcConfig
                {
                        get;
                        set;
                }

                /// <summary>
                /// 广播配置
                /// </summary>
                public RpcBroadcastConfig BroadcastConfig
                {
                        get;
                        set;
                }
                private static bool _CheckSendParam(TaskSendParam param, object parent, out string error)
                {
                        if (parent == null)
                        {
                                error = null;
                                return true;
                        }
                        TaskSendType sendType = TaskSendType.指令;
                        if (parent is AutoTask i)
                        {
                                sendType = i.SendType;
                        }
                        else if (parent is AutoTaskSetParam k)
                        {
                                sendType = k.SendType;
                        }
                        else
                        {
                                error = null;
                                return true;
                        }
                        if (sendType == TaskSendType.指令)
                        {
                                if (param.RpcConfig == null)
                                {
                                        error = "rpc.task.rpc.param.null";
                                        return false;
                                }
                                error = null;
                                return true;
                        }
                        else if (sendType == TaskSendType.URI)
                        {
                                if (param.HttpConfig == null)
                                {
                                        error = "rpc.task.http.param.null";
                                        return false;
                                }
                                error = null;
                                return true;
                        }
                        else if (param.BroadcastConfig == null)
                        {
                                error = "rpc.task.broadcast.param.null";
                                return false;
                        }
                        error = null;
                        return true;
                }
        }
}
