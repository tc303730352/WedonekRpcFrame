using System.Collections.Generic;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Balanced
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

                public RandomWeightBalanced(BasicConfig[] server)
                {
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

                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        return this._Server.DistributeNode(config, out server);
                }
        }
}
