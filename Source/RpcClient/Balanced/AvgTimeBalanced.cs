using System.Linq;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Balanced
{
        internal class AvgTimeBalanced : IBalanced
        {
                private static readonly int[] _EchelonVal = RpcClient.Config.GetConfigVal("AvgEchelonVal", new int[] {
                        50,
                        100,
                        150,
                        200,
                        250,
                        350,
                        500,
                        int.MaxValue
                });
                private readonly IBalanced _Server = null;
                public AvgTimeBalanced(long[] server)
                {
                        this._Server = this._Init(server);
                }
                private IBalanced _Init(long[] serverId)
                {
                        BasicConfig[] server = serverId.ConvertAll(a =>
                          {
                                  return new BasicConfig
                                  {
                                          serverId = a,
                                          weight = _calculationEchelon(a)
                                  };
                          });
                        int[] group = server.Select(a => a.weight).Distinct().OrderBy(a => a).ToArray();
                        if (group.Length == 1)
                        {
                                return new AverageBalanced(server.ConvertAll(a => a.serverId));
                        }
                        else
                        {
                                int len = group.Length;
                                group.ForEach((a, i) =>
                                {
                                        int val = len - i;
                                        server.ForEach(b => b.weight == a, b =>
                                        {
                                                b.weight = val;
                                        });
                                });
                                return new WeightBalanced(server);
                        }
                }
                private static int _calculationEchelon(long serverId)
                {
                        int avg = RemoteServerCollect.GetAvgTime(serverId);
                        return _EchelonVal.FindIndex(a => a >= avg);
                }
                public BalancedType BalancedType => BalancedType.avgtime;

                public bool DistributeNode(IRemoteConfig config, out IRemote server)
                {
                        return this._Server.DistributeNode(config, out server);
                }
        }
}
