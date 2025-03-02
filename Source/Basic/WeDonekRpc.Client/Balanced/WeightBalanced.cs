using System.Collections.Generic;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Balanced
{
    /// <summary>
    /// 权重负载
    /// </summary>
    internal class WeightBalanced : IBalanced
    {
        public BalancedType BalancedType
        {
            get;
        } = BalancedType.weight;
        private readonly IBalanced _Server = null;
        private readonly long[] _ServerId;
        public WeightBalanced (BasicConfig[] server, BalancedType type = BalancedType.weight)
        {
            this._ServerId = server.ConvertAll(c => c.serverId);
            this.BalancedType = type;
            List<long> list = new List<long>();
            server.ForEach(a =>
            {
                if (a.weight == 1)
                {
                    list.Add(a.serverId);
                }
                else
                {
                    a.weight.For(k =>
                    {
                        list.Add(a.serverId);
                    });
                }
            });
            this._Server = new AverageBalanced(list.Random());
        }
        public IRemoteCursor GetAllNode ()
        {
            return new RemoteCursor(this._ServerId);
        }
        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            return this._Server.DistributeNode(config, out server);
        }

    }
}
