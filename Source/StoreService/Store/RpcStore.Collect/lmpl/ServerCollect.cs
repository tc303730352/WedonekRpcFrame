using RpcStore.Collect.LocalEvent.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.Model.ServerGroup;
using RpcStore.RemoteModel.ServerConfig.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;
namespace RpcStore.Collect.lmpl
{

    internal class ServerCollect : IServerCollect
    {
        private readonly IRemoteServerConfigDAL _BasicDAL;
        private readonly ITransactionService _Tran;
        private readonly IRemoteServerGroupDAL _GroupDAL;
        private readonly IContainerDAL _ContainerDAL;
        public ServerCollect (IRemoteServerConfigDAL basicDAL,
            ITransactionService tran,
            IRemoteServerGroupDAL groupDAL,
            IContainerDAL containerDAL)
        {
            this._Tran = tran;
            this._BasicDAL = basicDAL;
            this._GroupDAL = groupDAL;
            this._ContainerDAL = containerDAL;
        }

        public bool CheckIsExists (long sysTypeId)
        {
            return this._BasicDAL.CheckIsExists(sysTypeId);
        }
        public Dictionary<int, int> GetReginServerNum (int[] regionId)
        {
            return this._BasicDAL.GetReginServerNum(regionId);
        }

        public bool SetState (RemoteServerConfigModel config, RpcServiceState state)
        {
            if (config.ServiceState == state)
            {
                return false;
            }
            else if (state == RpcServiceState.正常)
            {
                if (this._BasicDAL.CheckServerCode(config.ServerCode, config.VerNum))
                {
                    throw new ErrorException("rpc.store.server.code.repeat");
                }
            }
            this._BasicDAL.SetState(config.Id, state);
            config.ServiceState = state;
            return true;
        }
        public long Add (ServiceAddDatum config)
        {
            if (this._BasicDAL.CheckServerName(config.ServerName))
            {
                throw new ErrorException("rpc.store.server.name.repeat");
            }
            this.CheckServerPort(config.ServerMac, config.ServerPort);
            RemoteServerConfigModel add = config.ConvertMap<ServiceAddDatum, RemoteServerConfigModel>();
            int? index = this._BasicDAL.GetServiceIndex(add.SystemType, add.ServerMac);
            add.ServerIndex = index.HasValue ? index.Value + 1 : 0;
            using (ILocalTransaction tran = this._Tran.ApplyTran())
            {
                long id = this._BasicDAL.Add(add);
                this._GroupDAL.AddHold(add.HoldRpcMerId, new RemoteServerGroup
                {
                    RegionId = config.RegionId,
                    ServerId = id,
                    SystemType = config.SystemType,
                    ServiceType = config.ServiceType,
                    TypeVal = config.SystemTypeVal
                });
                tran.Commit();
                return id;
            }
        }

        public bool Set (RemoteServerConfigModel config, ServerSetDatum set)
        {
            if (set.IsEquals(config))
            {
                return false;
            }
            else if (set.ServerName != config.ServerName && this._BasicDAL.CheckServerName(config.ServerName))
            {
                throw new ErrorException("rpc.store.server.name.repeat");
            }
            else if (set.ServerPort != config.ServerPort || set.ServerMac != config.ServerMac)
            {
                this.CheckServerPort(set.ServerMac, set.ServerPort);
            }
            ServerConfigSet setDatum = set.ConvertMap<ServerSetDatum, ServerConfigSet>();
            if (config.HoldRpcMerId == set.HoldRpcMerId)
            {
                this._BasicDAL.SetService(config.Id, setDatum);
                return true;
            }
            RemoteServerGroupModel source = this._GroupDAL.Find(config.HoldRpcMerId, config.Id);
            RemoteServerGroupModel remoteGroup = this._GroupDAL.Find(set.HoldRpcMerId, config.Id);
            using (ILocalTransaction tran = this._Tran.ApplyTran())
            {
                this._BasicDAL.SetService(config.Id, setDatum);
                if (source != null)
                {
                    this._GroupDAL.SetIsHold(source.Id, false);
                }
                if (remoteGroup == null)
                {
                    this._GroupDAL.AddHold(set.HoldRpcMerId, new RemoteServerGroup
                    {
                        RegionId = config.RegionId,
                        ServerId = config.Id,
                        SystemType = config.SystemType,
                        TypeVal = set.SystemTypeVal
                    });
                }
                else
                {
                    this._GroupDAL.SetIsHold(remoteGroup.Id, true);
                }
                tran.Commit();
                return true;
            }
        }

        public void Delete (RemoteServerConfigModel config)
        {
            long[] ids = this._GroupDAL.GetIds(config.Id);
            if (ids.IsNull() && !config.IsContainer)
            {
                this._BasicDAL.Delete(config.Id);
                return;
            }
            using (ILocalTransaction tran = this._Tran.ApplyTran())
            {
                if (!ids.IsNull())
                {
                    this._GroupDAL.Delete(ids);
                }
                if (config.IsContainer)
                {
                    this._ContainerDAL.Delete(config.ContainerId.Value);
                }
                this._BasicDAL.Delete(config.Id);
                tran.Commit();
            }
            new ServerEvent("Delete").AsyncPublic();
        }
        public ServerConfigDatum[] Query (ServerConfigQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        public Dictionary<long, int> GetServerNum (long[] typeId, RpcServiceState[] states)
        {
            if (typeId.Length == 0)
            {
                return null;
            }
            return this._BasicDAL.GetServerNum(typeId, states);
        }
        public ServerConfigDatum[] Gets (long[] ids)
        {
            if (ids.Length == 0)
            {
                return new ServerConfigDatum[0];
            }
            return this._BasicDAL.GetServices(ids);
        }
        public RemoteServerConfigModel Get (long id)
        {
            RemoteServerConfigModel config = this._BasicDAL.Get(id);
            if (config == null)
            {
                throw new ErrorException("rpc.store.server.not.find");
            }
            return config;
        }

        public bool CheckIsOnline (long id)
        {
            return this._BasicDAL.CheckIsOnline(id);
        }

        public void CheckServerPort (string mac, int serverPort)
        {
            if (this._BasicDAL.CheckServerPort(mac, serverPort))
            {
                throw new ErrorException("rpc.store.server.port.repeat");
            }
        }

        public bool CheckIsExistsByGroup (long groupId)
        {
            return this._BasicDAL.CheckIsExistsByGroup(groupId);
        }

        public bool CheckRegion (int regionId)
        {
            return this._BasicDAL.CheckRegion(regionId);
        }
        public ServerItem[] GetItems (ServerConfigQuery query)
        {
            return this._BasicDAL.GetItems(query);
        }
        public SystemTypeVerNum[] GetAllVerNums (long[] ids)
        {
            return this._BasicDAL.GetAllVerNums(ids).OrderByDescending(a => a.VerNum).ToArray();
        }
        public SystemTypeVerNum[] GetVerNums (long[] ids)
        {
            return this._BasicDAL.GetVerNums(ids);
        }
        public BasicService[] GetBasics (ServerConfigQuery query)
        {
            return this._BasicDAL.GetBasics(query);
        }
        public string GetName (long serverId)
        {
            return this._BasicDAL.GetName(serverId);
        }
        public Dictionary<long, string> GetNames (long[] ids)
        {
            if (ids.IsNull())
            {
                return null;
            }
            return this._BasicDAL.GetNames(ids);
        }

        public Dictionary<long, int> GetContainerServerNum (long[] groupId, RpcServiceState[] states)
        {
            return this._BasicDAL.GetContainerServerNum(groupId, states);
        }

        public bool CheckIsExistByContainer (long containerGroupId)
        {
            return this._BasicDAL.CheckIsExistByContainer(containerGroupId);
        }

        public bool SetVerNum (RemoteServerConfigModel config, int verNum)
        {
            if (config.VerNum == verNum)
            {
                return false;
            }
            this._BasicDAL.SetVerNum(config.Id, verNum);
            return true;
        }
    }
}
