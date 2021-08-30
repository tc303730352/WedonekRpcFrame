using System;

using RpcModel;

using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Collect
{
        internal class BroadcastErrorCollect
        {
                private static readonly DelayDataSave<RemoteErrorLog> _SaveErrorLog = null;

                static BroadcastErrorCollect()
                {
                        _SaveErrorLog = new DelayDataSave<RemoteErrorLog>(new SaveDelayData<RemoteErrorLog>(_SaveLog), Tools.GetRandom(120, 180), 15);
                }

                private static void _SaveLog(ref RemoteErrorLog[] datas)
                {
                        if (!new DAL.BroadcastErrorLogDAL().AddErrorLog(datas))
                        {
                                throw new ErrorException("sync.save.broadcast.error");
                        }
                }
                public static void AddErrorLog(RemoteBroadcast data, string error)
                {
                        _SaveErrorLog.AddData(new RemoteErrorLog
                        {
                                Id = Tools.NewGuid(),
                                RpcMerId = data.Msg.RpcMerId,
                                ErrorCode = ErrorManage.GetErrorCode(error),
                                MsgKey = data.Msg.Datum.MsgKey,
                                ServerId = data.ServerId,
                                SysTypeVal = data.TypeVal ?? string.Empty,
                                MsgSource = data.Msg.Source.ToJson(),
                                MsgBody = data.Msg.Datum.MsgBody,
                                AddTime = DateTime.Now
                        });
                }


                internal static void AddErrorLog(BroadcastBody msg, string error)
                {
                        if (!msg.ServerId.IsNull())
                        {
                                _AddErrorLog(msg, msg.ServerId, error);
                        }
                        else if (!msg.Dictate.IsNull())
                        {
                                _AddErrorLog(msg, msg.Dictate, error);
                        }
                        else
                        {
                                _AddErrorLog(msg, error);
                        }
                }
                private static void _AddErrorLog(BroadcastBody msg, string error)
                {
                        _SaveErrorLog.AddData(new RemoteErrorLog
                        {
                                Id = Tools.NewGuid(),
                                RpcMerId = msg.RpcMerId,
                                ErrorCode = ErrorManage.GetErrorCode(error),
                                MsgKey = msg.Datum.MsgKey,
                                MsgSource = msg.Source.ToJson(),
                                MsgBody = msg.Datum.MsgBody,
                                AddTime = DateTime.Now
                        });
                }
                private static void _AddErrorLog(BroadcastBody msg, string[] types, string error)
                {
                        types.ForEach(a =>
                        {
                                _SaveErrorLog.AddData(new RemoteErrorLog
                                {
                                        Id = Tools.NewGuid(),
                                        RpcMerId = msg.RpcMerId,
                                        ErrorCode = ErrorManage.GetErrorCode(error),
                                        MsgKey = msg.Datum.MsgKey,
                                        SysTypeVal = a,
                                        MsgSource = msg.Source.ToJson(),
                                        MsgBody = msg.Datum.MsgBody,
                                        AddTime = DateTime.Now
                                });
                        });
                }
                private static void _AddErrorLog(BroadcastBody msg, long[] serverId, string error)
                {
                        serverId.ForEach(a =>
                        {
                                _SaveErrorLog.AddData(new RemoteErrorLog
                                {
                                        Id = Tools.NewGuid(),
                                        RpcMerId = msg.RpcMerId,
                                        ErrorCode = ErrorManage.GetErrorCode(error),
                                        MsgKey = msg.Datum.MsgKey,
                                        ServerId = a,
                                        MsgSource = msg.Source.ToJson(),
                                        MsgBody = msg.Datum.MsgBody,
                                        AddTime = DateTime.Now
                                });
                        });
                }




                internal static void AddErrorLog(BroadcastMsg msg, MsgSource source, string error)
                {
                        _SaveErrorLog.AddData(new RemoteErrorLog
                        {
                                Id = Tools.NewGuid(),
                                RpcMerId = msg.RpcMerId == 0 ? source.RpcMerId : msg.RpcMerId,
                                ErrorCode = ErrorManage.GetErrorCode(error),
                                MsgKey = msg.MsgKey,
                                MsgSource = source.ToJson(),
                                MsgBody = msg.ToJson(),
                                AddTime = DateTime.Now
                        });
                }

                internal static void AddErrorLog(long merId, BroadcastDatum msg, string typeVal, string error)
                {
                        _SaveErrorLog.AddData(new RemoteErrorLog
                        {
                                Id = Tools.NewGuid(),
                                RpcMerId = merId,
                                ErrorCode = ErrorManage.GetErrorCode(error),
                                MsgKey = msg.MsgKey,
                                ServerId = 0,
                                SysTypeVal = typeVal,
                                MsgBody = msg.ToJson(),
                                AddTime = DateTime.Now
                        });
                }
        }
}
