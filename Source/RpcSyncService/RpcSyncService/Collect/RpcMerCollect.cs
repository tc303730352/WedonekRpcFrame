using System.Collections.Concurrent;

using RpcClient.Broadcast;

using RpcSyncService.Controller;

using RpcHelper;

namespace RpcSyncService.Collect
{
        internal class RpcMerCollect
        {
                private static readonly ConcurrentDictionary<long, RpcMerController> _RpcMer = new ConcurrentDictionary<long, RpcMerController>();

                static RpcMerCollect()
                {
                        AutoClearDic.AutoClear(_RpcMer);
                        RpcClient.RpcClient.Route.AddRoute<RefreshMer>("RefreshMer", _Refresh);
                }

                private static void _Refresh(RefreshMer obj)
                {
                        if (_RpcMer.TryGetValue(obj.RpcMerId, out RpcMerController mer))
                        {
                                mer.ResetLock();
                        }
                        RemoteServerGroupCollect.Refresh(obj.RpcMerId);
                }

                public static bool GetRpcMer(long merId, out RpcMerController mer)
                {
                        if (!_RpcMer.TryGetValue(merId, out mer))
                        {
                                mer = _RpcMer.GetOrAdd(merId, new RpcMerController(merId));
                        }
                        if (!mer.Init())
                        {
                                _RpcMer.TryRemove(merId, out _);
                                mer.Dispose();
                                return false;
                        }
                        return mer.IsInit;
                }
        }
}
