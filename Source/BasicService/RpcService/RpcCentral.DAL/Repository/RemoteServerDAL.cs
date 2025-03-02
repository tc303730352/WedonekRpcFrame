using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class RemoteServerDAL : IRemoteServerDAL
    {
        private readonly IRepository<RemoteServerConfig> _Db;
        public static readonly RpcServiceState[] _EnableState = new RpcServiceState[]
        {
            RpcServiceState.待启用,
            RpcServiceState.正常,
            RpcServiceState.下线
        };
        public RemoteServerDAL (IRepository<RemoteServerConfig> db)
        {
            this._Db = db;
        }

        public int GetVerNum (long id)
        {
            return this._Db.Get(a => a.Id == id, a => a.VerNum);
        }
        public RemoteServerModel GetRemoteServer (long id)
        {
            return this._Db.Get<RemoteServerModel>(a => a.Id == id);
        }
        public BasicServer[] GetRemoteServerConfig (long[] ids)
        {
            return this._Db.Gets<BasicServer>(a => ids.Contains(a.Id) && a.ServiceState == RpcServiceState.正常);
        }
        public long[] LoadServer (int serverIndex)
        {
            return this._Db.Gets(c => c.BindIndex == serverIndex && c.IsOnline && _EnableState.Contains(c.ServiceState), c => c.Id);
        }
        public void SetContainerId (long serverId, long containerId)
        {
            if (!this._Db.Update(a => a.ContainerId == containerId, a => a.Id == serverId))
            {
                throw new ErrorException("rpc.server.container.id.set.fail");
            }
        }
        public void SetApiVer (long serverId, int ver)
        {
            if (!this._Db.Update(new RemoteServerConfig
            {
                Id = serverId,
                ApiVer = ver
            }, new string[] {
                "ApiVer"
            }))
            {
                throw new ErrorException("rpc.server.apiver.set.fail");
            }
        }
        public bool ServerOnline (long serverId, int serverIndex)
        {
            return this._Db.Update(a => new RemoteServerConfig
            {
                IsOnline = true,
                BindIndex = serverIndex
            }, a => a.Id == serverId && a.IsOnline == false);
        }
        public void SetConIp (long serverId, string conIp)
        {
            if (!this._Db.Update(new RemoteServerConfig
            {
                Id = serverId,
                ConIp = conIp
            }, new string[] {
                "ConIp"
            }))
            {
                throw new ErrorException("rpc.server.conIp.set.fail");
            }
        }
        public bool ServerOffline (long serverId, int serverIndex)
        {
            return this._Db.Update(a => new RemoteServerConfig
            {
                IsOnline = false,
                LastOffliceDate = HeartbeatTimeHelper.CurrentDate
            }, a => a.Id == serverId && a.IsOnline == true && a.BindIndex == serverIndex);
        }

        public long FindServiceId (long systemType, string mac, int serverIndex)
        {
            return this._Db.Get(c => c.SystemType == systemType && c.ServerMac == mac && c.ServerIndex == serverIndex && c.IsContainer == false, c => c.Id);
        }
        public ServerCont FindService (ContainerGetArg arg)
        {
            return this._Db.Get<ServerCont>(a => a.ServerPort == arg.ServerPort &&
            a.SystemType == arg.SystemType &&
            a.HoldRpcMerId == arg.HoldRpcMerId &&
            a.ContainerGroupId == arg.ContainerGroupId &&
            a.IsContainer &&
            _EnableState.Contains(a.ServiceState));
        }

        public bool CheckContainerPort (long contGroupId, int serverPort)
        {
            return this._Db.IsExist(c => c.ServerPort == serverPort && c.ContainerGroupId == contGroupId && c.IsContainer && _EnableState.Contains(c.ServiceState));
        }
        public long Add (RemoteServerConfig add, string typeVal)
        {
            add.Id = IdentityHelper.CreateId();
            add.AddTime = DateTime.Now;
            ISqlQueue<RemoteServerConfig> queue = this._Db.BeginQueue();
            queue.Insert(add);
            queue.Insert(new RemoteServerGroup
            {
                Id = IdentityHelper.CreateId(),
                IsHold = true,
                RegionId = add.RegionId,
                ServerId = add.Id,
                RpcMerId = add.HoldRpcMerId,
                ServiceType = add.ServiceType,
                SystemType = add.SystemType,
                TypeVal = typeVal,
                Weight = 1
            });
            if (queue.Submit() < 0)
            {
                throw new ErrorException("rpc.server.add.fail");
            }
            return add.Id;
        }
    }
}
