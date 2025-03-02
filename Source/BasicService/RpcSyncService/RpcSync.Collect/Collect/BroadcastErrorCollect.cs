using RpcSync.Collect.linq;
using RpcSync.Collect.Model;
using RpcSync.DAL;
using RpcSync.Model.DB;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;

namespace RpcSync.Collect.Collect
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class BroadcastErrorCollect : IBroadcastErrorCollect
    {
        private readonly DelayDataSave<BroadcastErrorLogModel> _SaveErrorLog = null;
        private readonly IIocService _Unity;
        private static readonly string _DeadError = "rpc.queue.dead.msg";
        static BroadcastErrorCollect ()
        {
        }
        public BroadcastErrorCollect (IIocService unity)
        {
            this._Unity = unity;
            this._SaveErrorLog = new DelayDataSave<BroadcastErrorLogModel>(new SaveDelayData<BroadcastErrorLogModel>(this._SaveLog), Tools.GetRandom(120, 180), 15);
        }
        private void _SaveLog (ref BroadcastErrorLogModel[] datas)
        {
            using (IocScope scope = this._Unity.CreateTempScore())
            {
                IBroadcastErrorLogDAL logDAL = scope.Resolve<IBroadcastErrorLogDAL>();
                logDAL.AddErrorLog(datas);
            }
        }

        public void AddErrorLog (QueueRemoteMsg msg, string routeKey, BroadcastType msgType)
        {
            MsgSource source = msg.Msg.Source;
            this._SaveErrorLog.AddData(new BroadcastErrorLogModel
            {
                Id = IdentityHelper.CreateId(),
                RpcMerId = source.RpcMerId,
                ServerId = 0,
                BroadcastType = msgType,
                Error = _DeadError,
                RouteKey = routeKey,
                MsgKey = msg.Type,
                SysTypeVal = string.Empty,
                SourceId = source.ServerId,
                MsgSource = source,
                MsgBody = msg.Msg.ToMsgBody(),
                AddTime = DateTime.Now
            });
        }

        public void AddErrorLog (RemoteBroadcast data, string error)
        {
            MsgSource source = data.Msg.Source;
            this._SaveErrorLog.AddData(new BroadcastErrorLogModel
            {
                Id = IdentityHelper.CreateId(),
                RpcMerId = data.Msg.RpcMerId,
                BroadcastType = BroadcastType.默认,
                Error = error,
                MsgKey = data.Msg.Datum.MsgKey,
                ServerId = data.ServerId,
                SysTypeVal = data.TypeVal ?? string.Empty,
                SourceId = source.ServerId,
                MsgSource = source,
                MsgBody = data.Msg.Datum.MsgBody,
                AddTime = DateTime.Now
            });
        }


        public void AddErrorLog (BroadcastBody msg, string error)
        {
            if (!msg.ServerId.IsNull())
            {
                this._AddErrorLog(msg, msg.ServerId, error);
            }
            else if (!msg.Dictate.IsNull())
            {
                this._AddErrorLog(msg, msg.Dictate, error);
            }
            else
            {
                this._AddErrorLog(msg, error);
            }
        }
        private void _AddErrorLog (BroadcastBody msg, string error)
        {
            this._SaveErrorLog.AddData(new BroadcastErrorLogModel
            {
                Id = IdentityHelper.CreateId(),
                RpcMerId = msg.RpcMerId,
                BroadcastType = BroadcastType.默认,
                Error = error,
                MsgKey = msg.Datum.MsgKey,
                MsgSource = msg.Source,
                MsgBody = msg.Datum.MsgBody,
                AddTime = DateTime.Now
            });
        }
        private void _AddErrorLog (BroadcastBody msg, string[] types, string error)
        {
            types.ForEach(a =>
            {
                this._SaveErrorLog.AddData(new BroadcastErrorLogModel
                {
                    Id = IdentityHelper.CreateId(),
                    RpcMerId = msg.RpcMerId,
                    BroadcastType = BroadcastType.默认,
                    Error = error,
                    MsgKey = msg.Datum.MsgKey,
                    SysTypeVal = a,
                    MsgSource = msg.Source,
                    SourceId = msg.Source.ServerId,
                    MsgBody = msg.Datum.MsgBody,
                    AddTime = DateTime.Now
                });
            });
        }
        private void _AddErrorLog (BroadcastBody msg, long[] serverId, string error)
        {
            serverId.ForEach(a =>
            {
                this._SaveErrorLog.AddData(new BroadcastErrorLogModel
                {
                    Id = IdentityHelper.CreateId(),
                    RpcMerId = msg.RpcMerId,
                    BroadcastType = BroadcastType.默认,
                    Error = error,
                    MsgKey = msg.Datum.MsgKey,
                    ServerId = a,
                    SourceId = msg.Source.ServerId,
                    MsgSource = msg.Source,
                    MsgBody = msg.Datum.MsgBody,
                    AddTime = DateTime.Now
                });
            });
        }
    }
}
