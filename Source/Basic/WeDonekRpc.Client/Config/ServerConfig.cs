using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Config
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class ServerConfig : IServerConfig
    {
        private readonly RpcServerConfig _Config;

        public ServerConfig ()
        {
            this._Config = RpcStateCollect.ServerConfig;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string Name => this._Config.Name;
        /// <summary>
        /// 服务器ID
        /// </summary>
        public long ServerId => this._Config.ServerId;
        /// <summary>
        /// 服务端Ip
        /// </summary>
        public string ServerIp => this._Config.ServerIp;

        /// <summary>
        /// 服务端端口
        /// </summary>
        public int ServerPort => this._Config.ServerPort;
        /// <summary>
        /// 所属服务组
        /// </summary>
        public string GroupTypeVal => this._Config.GroupTypeVal;
        /// <summary>
        /// 服务组Id
        /// </summary>
        public long GroupId => this._Config.GroupId;
        /// <summary>
        /// 节点类型ID
        /// </summary>
        public long SystemType => this._Config.SystemType;
        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState ServiceState => this._Config.ServiceState;
        /// <summary>
        /// 所属区域
        /// </summary>
        public int RegionId => this._Config.RegionId;

        /// <summary>
        /// 配置权限值
        /// </summary>
        public short ConfigPrower => this._Config.ConfigPrower;
        /// <summary>
        /// 节点版本号
        /// </summary>
        public long VerNum => this._Config.VerNum;
    }
}
