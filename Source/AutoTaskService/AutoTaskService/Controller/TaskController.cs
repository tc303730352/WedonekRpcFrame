using System;

using AutoTaskService.Model;

using RpcClient.Collect;

using RpcModel;

using RpcHelper;
using RpcHelper.TaskTools;
namespace AutoTaskService.Controller
{
        /// <summary>
        /// 任务控制器
        /// </summary>
        internal class TaskController : System.IDisposable
        {
                public TaskController(long id)
                {
                        this.Id = id;
                }
                public long RpcMerId
                {
                        get;
                        private set;
                }
                public long Id { get; }

                public TaskSendType SendType
                {
                        get;
                        private set;
                }
                public int VerNum
                {
                        get;
                        private set;
                }
                public SendParam SendParam
                {
                        get;
                        private set;
                }
                private readonly string _TakeName = null;

                public bool Init(out string error)
                {
                        if (!new DAL.AutoTaskDAL().GetTask(this.Id, out RemoteTask task))
                        {
                                error = "rpc.task.get.error";
                                return false;
                        }
                        else if (task == null)
                        {
                                error = "rpc.task.not.find";
                                return false;
                        }
                        else
                        {
                                this.RpcMerId = task.RpcMerId;
                                this.VerNum = task.VerNum;
                                this.SendType = task.SendType;
                                this.SendParam = task.SendParam;
                                TaskManage.AddTask(new TaskHelper(task.TaskName, task.TaskType, new TimeSpan(0, 0, task.TaskTimeSpan), new Action(this._ExecTask), this.Id.ToString(), task.TaskPriority));
                                error = null;
                                return true;
                        }
                }

                private void _ExecTask()
                {
                        if (this.SendType == TaskSendType.指令)
                        {
                                this._SendDictate(this.SendParam.RpcConfig);
                        }
                        else if (this.SendType == TaskSendType.广播)
                        {
                                this._SendBroadcast(this.SendParam.BroadcastConfig);
                        }
                        else if (this.SendType == TaskSendType.URI)
                        {
                                this._SendHttp(this.SendParam.HttpConfig);
                        }
                }
                private void _SendDictate(RpcParamConfig param)
                {
                        RpcSendConfig config = param.SendConfig;
                        IRemoteConfig send = new IRemoteConfig(config.SysDictate, false)
                        {
                                IsAllowRetry = config.IsRetry,
                                IdentityColumn = config.IdentityColumn,
                                LockColumn = config.LockColumn,
                                LockType = config.LockType,
                                TransmitType = config.TransmitType,
                                TransmitId = config.TransmitId,
                                IsSync = !config.LockColumn.IsNull(),
                                ZIndexBit = config.ZIndexBit,
                                RpcMerId = this.RpcMerId,
                                RegionId = config.RegionId,
                                SystemType = param.SystemType
                        };
                        if (!RemoteCollect.Send(send, param.MsgBody, out string error))
                        {
                                new WarnLog(error, "发送指令错误!")
                                {
                                        { "TaskName",this._TakeName},
                                        { "Param",param.ToJson()}
                                }.Save();
                        }
                }
                private void _SendHttp(HttpParamConfig param)
                {
                        if (!HttpHelper.SendRequest(new Uri(param.Uri), param.PostParam, out HttpResponseRes res, new HttpRequestSet(param.ReqType)
                        {
                                SubmitMethod = param.RequestMethod
                        }))
                        {
                                new WarnLog("rpc.task.http.error", "发送Http请求错误!")
                                {
                                        { "TaskName",this._TakeName},
                                        { "Param",param.ToJson()}
                                }.Save();
                        }
                        else if (res.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                                new DebugLog("HTTP请求返回失败!")
                                {
                                        { "TaskName",this._TakeName},
                                        { "Param",param.ToJson()},
                                        { "HttpStatus",res.StatusCode}
                                }.Save();
                        }
                }
                private void _SendBroadcast(RpcBroadcastConfig param)
                {
                        RpcSendConfig config = param.SendConfig;
                        IRemoteBroadcast send = new IRemoteBroadcast(config.SysDictate, param.IsOnly, param.TypeVal)
                        {
                                IsCrossGroup = param.IsCrossGroup,
                                RpcMerId = this.RpcMerId,
                                ServerId = param.ServerId,
                                BroadcastType = param.BroadcastType,
                                IsExclude = param.IsExclude,
                                RegionId = config.RegionId,
                                IsOnly = param.IsOnly,
                                TypeVal = param.TypeVal,
                                RemoteConfig = new IRemoteConfig
                                {
                                        IsAllowRetry = config.IsRetry,
                                        IdentityColumn = config.IdentityColumn,
                                        LockColumn = config.LockColumn,
                                        LockType = config.LockType,
                                        TransmitType = config.TransmitType,
                                        TransmitId = config.TransmitId,
                                        IsSync = !config.LockColumn.IsNull(),
                                        ZIndexBit = config.ZIndexBit,
                                        RpcMerId = this.RpcMerId
                                }
                        };
                        RemoteCollect.BroadcastMsg(send, param.MsgBody);
                }
                public void Dispose()
                {
                        TaskManage.RemoveTask(this.Id.ToString());
                }
                public bool Reset(out string error)
                {
                        TaskManage.RemoveTask(this.Id.ToString());
                        return this.Init(out error);
                }
        }
}
