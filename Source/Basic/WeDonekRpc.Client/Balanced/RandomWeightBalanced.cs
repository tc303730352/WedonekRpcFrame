using System.Collections.Generic;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Balanced
{
    /// <summary>
    /// 按照权重分布节点后随机
    /// </summary>
    internal class RandomWeightBalanced : IBalanced
    {
        public BalancedType BalancedType
        {
            get;
        } = BalancedType.randomWeight;
        private readonly IBalanced _Server = null;
        private readonly long[] _ServiceId;
        public RandomWeightBalanced (BasicConfig[] server)
        {
            this._ServiceId = server.ConvertAll(c => c.serverId);
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
            this._Server = new RandomBalanced(list.ToArray());
        }
        public IRemoteCursor GetAllNode ()
        {
            return new RemoteCursor(this._ServiceId);
        }
        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            return this._Server.DistributeNode(config, out server);
        }
    }
}
