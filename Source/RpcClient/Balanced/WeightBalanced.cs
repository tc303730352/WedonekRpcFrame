using System.Collections.Generic;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Balanced
{
        /// <summary>
        /// 权重
        /// </summary>
        internal class WeightBalanced : IBalanced
        {
                public BalancedType BalancedType
                {
                        get;
                } = BalancedType.weight;
                private readonly IBalanced _Server = null;

                public WeightBalanced(BasicConfig[] server, BalancedType type = BalancedType.weight)
                {
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

                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        return this._Server.DistributeNode(config, out server);
                }
        }
}
