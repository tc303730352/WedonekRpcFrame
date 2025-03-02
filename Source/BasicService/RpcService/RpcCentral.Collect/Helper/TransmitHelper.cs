using RpcCentral.Collect.Model;
using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Helper
{
    internal class TransmitHelper : ITransmitHelper
    {
        private readonly IServerTransmitConfigDAL _TransmitConfig;

        public TransmitHelper (IServerTransmitConfigDAL transmitConfig)
        {
            this._TransmitConfig = transmitConfig;
        }
        public Transmit[] GetTransmits (long rpcMerId, long systemType)
        {
            ServerTransmitScheme[] configs = this._TransmitConfig.Gets(systemType, rpcMerId);
            if (configs.Length == 0)
            {
                return new Transmit[0];
            }
            List<Transmit> list = [];
            configs.ForEach(a =>
            {
                this._InitTransmit(a, list);
            });
            return list.OrderByDescending(a => a.Sort).Distinct().ToArray();
        }
        private void _InitTransmit (ServerTransmitScheme scheme, List<Transmit> list)
        {
            foreach (ServerTransmitConfig config in scheme.Transmit)
            {
                config.TransmitConfig.ForEach(d =>
                {
                    list.Add(new Transmit
                    {
                        Range = d.Range,
                        Scheme = scheme.Scheme,
                        TransmitType = scheme.TransmitType,
                        Ver = scheme.VerNum,
                        Value = d.Value,
                        ServerCode = config.ServerCode,
                        Sort = 100
                    });
                });
            }
        }
    }
}
