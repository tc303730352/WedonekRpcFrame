using RpcCentral.Collect;
using RpcCentral.Collect.Controller;
using RpcCentral.Collect.Model;
using RpcCentral.Common;
using RpcCentral.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Service
{
    internal class GetServiceListService : IGetServiceListService
    {
        private readonly ISystemTypeCollect _SystemType;
        private readonly IRpcServerConfigCollect _ServerConfig;
        private readonly IVerConfigCollect _VerConfig;
        private readonly ITransmitConfigCollect _TransmitConfig;
        private readonly IRpcConfigCollect _RpcConfig;
        public GetServiceListService (ISystemTypeCollect systemType,
            IRpcServerConfigCollect serverConfig,
            IVerConfigCollect verConfig,
            IRpcConfigCollect rpcConfig,
            ITransmitConfigCollect transmitConfig)
        {
            this._RpcConfig = rpcConfig;
            this._SystemType = systemType;
            this._ServerConfig = serverConfig;
            this._VerConfig = verConfig;
            this._TransmitConfig = transmitConfig;
        }
        private string _TransmitVer;
        private int _ConfigVer;
        private long _RpcMerId;
        private long _SysTypeId;
        private RpcConfigController _Config;
        private RpcServerConfigController _Server;
        private SystemTypeDatum _SysType;

        public GetServerListRes GetServerList (GetServerList obj, Source source)
        {
            this._TransmitVer = obj.TransmitVer;
            this._ConfigVer = obj.ConfigVer;
            this._SysType = this._SystemType.GetSystemType(obj.SystemType);
            this._SysTypeId = this._SysType.Id;
            this._Server = this._ServerConfig.Get(obj.RpcMerId, this._SysType.Id);
            if (this._Server.IsNull)
            {
                return new GetServerListRes
                {
                    BalancedType = BalancedType.single,
                    TransmitVer = this._TransmitVer,
                    ConfigVer = this._Server.Ver,
                    Servers = null
                };
            }
            this._RpcMerId = this._Server.HoldRpcMerId;
            RpcVerController ver = this._VerConfig.GetVer(this._RpcMerId, source.SystemTypeId, source.Ver);
            this._Config = this._RpcConfig.Get(this._RpcMerId, this._SysType.Id);
            int verNum = ver.GetVer(this._SysType.Id, this._Config.CurrentVer);
            if (obj.LimitRegionId.HasValue)
            {
                ServerConfig[] services = this._GetServers(obj.LimitRegionId.Value, verNum);
                return new GetServerListRes
                {
                    BalancedType = this._Config.BalancedType,
                    TransmitVer = this._TransmitVer,
                    ConfigVer = this._ConfigVer,
                    Servers = services
                };
            }
            else
            {
                ServerConfig[] locals = this._GetServers(source.RegionId, verNum, out ServerConfig[] backs);
                return new GetServerListRes
                {
                    BalancedType = this._Config.BalancedType,
                    TransmitVer = this._TransmitVer,
                    ConfigVer = this._ConfigVer,
                    Servers = locals,
                    BackUp = backs
                };
            }

        }
        private ServerConfig[] _GetServers (int regionId, int ver, out ServerConfig[] back)
        {
            TransmitConfigController transmit = this._TransmitConfig.GetTransmit(this._SysTypeId, this._RpcMerId);
            int curVer = this._Server.Ver + this._Config.Ver;
            if (!transmit.TryGets(ver, this._Config.CurrentVer, ref this._TransmitVer, out Transmit[] list) && curVer == this._ConfigVer)
            {
                back = null;
                return null;
            }
            this._ConfigVer = curVer;
            if (!this._Config.IsRegionIsolate)
            {
                back = null;
                return this._Format(this._Server.Gets(ver), list);
            }
            else if (this._Config.IsolateLevel)
            {
                back = null;
                return this._Format(this._Server.Gets(regionId, ver), list);
            }
            else
            {
                ServerConfig[] servers = this._Server.Gets(regionId, ver);
                if (servers.Length == 0)
                {
                    back = null;
                    return this._Format(this._Server.Gets(ver), list);
                }
                else
                {
                    back = this._Format(this._Server.GetsNoRegion(regionId, ver), list);
                    return this._Format(servers, list);
                }
            }
        }
        private ServerConfig[] _Format (ServerConfig[] servers, Transmit[] list)
        {
            if (servers != null && !list.IsNull())
            {
                servers.ForEach(c =>
                {
                    ServerTransmit[] t = list.ConvertMap<Transmit, ServerTransmit>(a => a.ServerCode == c.ServerCode);
                    if (t.Length != 0)
                    {
                        c.Transmit = t;
                        c.IsTransmit = true;
                    }
                });
            }
            return servers;
        }
        private ServerConfig[] _GetServers (int regionId, int ver)
        {
            TransmitConfigController transmit = this._TransmitConfig.GetTransmit(this._SysTypeId, this._RpcMerId);
            int curVer = this._Server.Ver + this._Config.Ver;
            if (!transmit.TryGets(ver, this._Config.CurrentVer, ref this._TransmitVer, out Transmit[] list) && curVer == this._ConfigVer)
            {
                return null;
            }
            else
            {
                this._ConfigVer = curVer;
                ServerConfig[] servers = this._Server.Gets(regionId, ver);
                return this._Format(servers, list);
            }
        }
    }
}
