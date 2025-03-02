using RpcCentral.Collect.Model;
using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcCentral.Collect.Controller
{
    public class RpcServerConfigController : DataSyncClass
    {
        private readonly long _RpcMerId;
        public RpcServerConfigController ( long rpcMerId, long sysTypeId )
        {
            this._RpcMerId = rpcMerId;
            this.SystemTypeId = sysTypeId;
        }
        public long SystemTypeId { get; private set; }
        private RpcServiceConfig[] _Server;

        public int Ver { get; private set; }
        /// <summary>
        /// 所属集群ID
        /// </summary>
        public long HoldRpcMerId { get; private set; }
        protected override void SyncData ()
        {
            using ( IocScope scope = UnityHelper.CreateTempScore() )
            {
                RemoteConfig[] list = scope.Resolve<IServerGroupCollect>().GetRemoteServer(this._RpcMerId, this.SystemTypeId);
                if ( list.IsNull() )
                {
                    this._Server = new RpcServiceConfig[0];
                    this.IsNull = true;
                    this.Ver = Tools.GetRandom();
                    return;
                }
                BasicServer[] servers = scope.Resolve<IRemoteServerDAL>().GetRemoteServerConfig(list.ConvertAll(a => a.ServerId));
                this._Server = servers.ConvertAll(a =>
                {
                    return new RpcServiceConfig
                    {
                        RegionId = a.RegionId,
                        ServerId = a.Id,
                        ServerCode = a.ServerCode,
                        Weight = list.Find(b => b.ServerId == a.Id, b => b.Weight, 1),
                        VerNum = a.VerNum
                    };
                });
                this.HoldRpcMerId = servers[0].HoldRpcMerId;
                this.IsNull = false;
                this.Ver = Tools.GetRandom(0, 9999);
            }
        }
        public bool IsNull
        {
            get;
            private set;
        }

        public ServerConfig[] Gets ( int regionId, int ver )
        {
            RpcServiceConfig[] services = this._Server.FindAll(c => c.RegionId == regionId && c.VerNum == ver);
            if ( services.IsNull() )
            {
                return Array.Empty<ServerConfig>();
            }
            return services.ConvertMap<RpcServiceConfig, ServerConfig>();
        }
        public ServerConfig[] GetsNoRegion ( int regionId, int ver )
        {
            RpcServiceConfig[] services = this._Server.FindAll(c => c.RegionId != regionId && c.VerNum == ver);
            if ( services.IsNull() )
            {
                return Array.Empty<ServerConfig>();
            }
            return services.ConvertMap<RpcServiceConfig, ServerConfig>();
        }
        public ServerConfig[] Gets ( int ver )
        {
            RpcServiceConfig[] services = this._Server.FindAll(c => c.VerNum == ver);
            if ( services.IsNull() )
            {
                return Array.Empty<ServerConfig>();
            }
            return services.ConvertMap<RpcServiceConfig, ServerConfig>();
        }
    }
}
