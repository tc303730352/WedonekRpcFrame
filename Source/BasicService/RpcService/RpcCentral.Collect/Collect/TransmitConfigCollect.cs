using System.Collections.Concurrent;
using RpcCentral.Collect.Controller;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class TransmitConfigCollect : ITransmitConfigCollect
    {
        private static readonly ConcurrentDictionary<string, TransmitConfigController> _Transmit = new ConcurrentDictionary<string, TransmitConfigController>();
        static TransmitConfigCollect ()
        {
            AutoResetDic.AutoReset(_Transmit);
        }
        public void Refresh (long systemType, long rpcMerId)
        {
            string key = string.Concat(systemType, "_", rpcMerId);
            if (_Transmit.TryGetValue(key, out TransmitConfigController transmit))
            {
                transmit.ResetLock();
            }
        }
        public TransmitConfigController GetTransmit (long systemType, long rpcMerId)
        {
            if (_GetTransmit(systemType, rpcMerId, out TransmitConfigController transmit))
            {
                return transmit;
            }
            throw new ErrorException(transmit.Error);
        }
        private static bool _GetTransmit (long systemType, long rpcMerId, out TransmitConfigController transmit)
        {
            string key = string.Concat(systemType, "_", rpcMerId);
            if (!_Transmit.TryGetValue(key, out transmit))
            {
                transmit = _Transmit.GetOrAdd(key, new TransmitConfigController(systemType, rpcMerId));
            }
            if (!transmit.Init())
            {
                _ = _Transmit.TryRemove(key, out transmit);
                transmit.Dispose();
                return false;
            }
            return transmit.IsInit;
        }

    }
}
