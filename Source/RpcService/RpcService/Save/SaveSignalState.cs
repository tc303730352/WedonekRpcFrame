using System;
using System.Linq;

using RpcModel.Server;

using RpcService.DAL;
using RpcService.Model.DAL_Model;

using RpcHelper;

namespace RpcService.Save
{
        internal class SaveSignalState
        {
                private static readonly IDelayDataSave<ServerSignalState> _SaveState = new DelayDataSave<ServerSignalState>(_Save, _Filter, 10, 5);

                private static void _Save(ref ServerSignalState[] datas)
                {
                        if (!new ServerSignalStateDAL().SyncState(datas))
                        {
                                throw new ErrorException("rpc.state.save.error");
                        }
                }

                private static void _Filter(ref ServerSignalState[] datas)
                {
                        if (datas.Length > 1)
                        {
                                Array.Reverse(datas);
                                datas = datas.Distinct().ToArray();
                        }
                }
                public static void Add(long serverId, RemoteState[] remotes)
                {
                        remotes.ForEach(a =>
                        {
                                _SaveState.AddData(new ServerSignalState
                                {
                                        AvgTime = a.AvgTime,
                                        ConNum = a.ConNum,
                                        UsableState = a.State,
                                        RemoteId = a.ServerId,
                                        ErrorNum = a.ErrorNum,
                                        ServerId = serverId,
                                        SendNum = a.SendNum
                                });
                        });
                }
        }
}
