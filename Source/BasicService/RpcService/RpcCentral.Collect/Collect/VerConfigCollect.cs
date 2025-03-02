using System.Collections.Concurrent;
using RpcCentral.Collect.Controller;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class VerConfigCollect : IVerConfigCollect
    {
        private static readonly ConcurrentDictionary<string, RpcVerController> _VerList = new ConcurrentDictionary<string, RpcVerController>();
        public RpcVerController GetVer (long rpcMerId, long sysTypeId, int verNum)
        {
            if (_GetVer(rpcMerId, sysTypeId, verNum, out RpcVerController ver))
            {
                return ver;
            }
            throw new ErrorException(ver.Error);
        }

        private static bool _GetVer (long rpcMerId, long sysTypeId, int verNum, out RpcVerController ver)
        {
            string key = string.Join("_", "Ver", rpcMerId, sysTypeId, verNum);
            if (!_VerList.TryGetValue(key, out ver))
            {
                ver = _VerList.GetOrAdd(key, new RpcVerController(rpcMerId, sysTypeId, verNum));
            }
            if (!ver.Init())
            {
                _ = _VerList.TryRemove(key, out ver);
                ver.Dispose();
                return false;
            }
            return ver.IsInit;
        }

        public void Refresh (long rpcMerId, long sysTypeId, int verNum)
        {
            string key = string.Join("_", "Ver", rpcMerId, sysTypeId, verNum);
            if (_VerList.TryGetValue(key, out RpcVerController ver))
            {
                ver.ResetLock();
            }
        }
    }
}
