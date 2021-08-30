using RpcClient.Balanced;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Collect
{
        internal class BalancedCollect
        {
                public static IBalanced GetBalanced(BalancedType bType, BasicConfig[] server)
                {
                        if (server.Length == 1)
                        {
                                return new SingleBalanced(server[0].serverId);
                        }
                        else if (bType == BalancedType.random)
                        {
                                return new RandomBalanced(server.ConvertAll(a => a.serverId));
                        }
                        else if (bType == BalancedType.weight)
                        {
                                return new WeightBalanced(server);
                        }
                        else if (bType == BalancedType.avgtime)
                        {
                                return new AvgTimeBalanced(server.ConvertAll(a => a.serverId));
                        }
                        else if (bType == BalancedType.randomWeight)
                        {
                                return new RandomWeightBalanced(server);
                        }
                        return new AverageBalanced(server.ConvertAll(a => a.serverId));
                }
        }
}
