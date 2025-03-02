using System.Collections.Generic;
using System.Linq;
using WeDonekRpc.Client.Remote.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Model
{
    /// <summary>
    /// 节点类型负载配置
    /// </summary>
    internal class BalancedConfig
    {
        public BalancedConfig (BalancedType type, ServerConfig[] configs)
        {
            this.BalancedType = type;
            configs.ForEach(a =>
            {
                this._Server.Add(a.ServerId, new BasicConfig
                {
                    serverId = a.ServerId,
                    weight = a.Weight
                });
            });
        }
        private readonly Dictionary<long, BasicConfig> _Server = new Dictionary<long, BasicConfig>();

        public BalancedType BalancedType
        {
            get;
        }
        public BasicConfig[] ToConfig ()
        {
            return this._Server.Values.ToArray();
        }
        public BasicConfig[] ToConfig (RangeServer[] server)
        {
            return server.ConvertAll(a => this._Server[a.ServerId]);
        }
        public BasicConfig[] ToConfig (long[] serverId)
        {
            return serverId.ConvertAll(a => this._Server[a]);
        }
    }
}
