using WeDonekRpc.Client.Balanced;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// 服务节点负载均衡构建服务
    /// </summary>
    internal class BalancedCollect
    {
        /// <summary>
        /// 根据负载类型生成一个负载接口服务
        /// </summary>
        /// <param name="bType">负载类型</param>
        /// <param name="server">负载配置列表</param>
        /// <returns>构建的负载均衡器</returns>
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
