using System.Linq;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Balanced
{
    /// <summary>
    /// 平均响应时间负载算法
    /// </summary>
    internal class AvgTimeBalanced : IBalanced
    {
        /// <summary>
        /// 平均响应时间的阶段值
        /// </summary>
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
        private readonly long[] _ServerId;
        public AvgTimeBalanced (long[] server)
        {
            this._ServerId = server;
            this._Server = this._Init(server);
        }
        private IBalanced _Init (long[] serverId)
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
        private static int _calculationEchelon (long serverId)
        {
            int avg = RemoteServerCollect.GetAvgTime(serverId);
            return _EchelonVal.FindIndex(a => a >= avg);
        }
        public BalancedType BalancedType => BalancedType.avgtime;

        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            return this._Server.DistributeNode(config, out server);
        }
        public IRemoteCursor GetAllNode ()
        {
            return new RemoteCursor(this._ServerId);
        }
    }
}
