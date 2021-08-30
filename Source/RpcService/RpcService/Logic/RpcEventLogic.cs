using System.Collections.Generic;

using RpcModel;

using RpcService.Collect;
using RpcService.Model;
using RpcService.RpcEvent;

namespace RpcService.Logic
{
        internal class RpcEventLogic
        {
                private static readonly Dictionary<string, IRpcEvent> _EventList = new Dictionary<string, IRpcEvent>();

                static RpcEventLogic()
                {
                        _EventList.Add("RefreshMerConfig", new RefreshMerConfig());
                        _EventList.Add("RefreshService", new RefreshService());
                        _EventList.Add("RefreshConfig", new RefreshConfig());
                        _EventList.Add("RefreshMer", new RefreshMer());
                        _EventList.Add("RefreshReduce", new RefreshReduce());
                        _EventList.Add("RefreshLimit", new RefreshLimit());
                        _EventList.Add("RefreshDictateLimit", new RefreshDictateLimit());
                        _EventList.Add("RefreshClientLimit", new RefreshClientLimit());
                }
                public static bool Execate(RefreshRpc obj, out string error)
                {
                        if (!RpcTokenCollect.GetOAuthToken(obj.TokenId, out RpcToken token, out error))
                        {
                                return false;
                        }
                        else if (_EventList.TryGetValue(obj.EventKey, out IRpcEvent rpc))
                        {
                                rpc.Refresh(token, obj.Param);
                        }
                        return true;
                }
        }
}
