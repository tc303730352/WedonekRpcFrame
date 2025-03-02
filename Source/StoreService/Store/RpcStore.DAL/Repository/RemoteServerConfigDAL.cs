using RpcStore.Model.DB;
using RpcStore.Model.ServerConfig;
using RpcStore.RemoteModel.ServerConfig.Model;
using SqlSugar;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class RemoteServerConfigDAL : IRemoteServerConfigDAL
    {
        private readonly IRepository<RemoteServerConfigModel> _BasicDAL;
        public RemoteServerConfigDAL (IRepository<RemoteServerConfigModel> dal)
        {
            this._BasicDAL = dal;
        }
        public bool CheckServerName (string name)
        {
            return this._BasicDAL.IsExist(c => c.ServerName == name);
        }
        public bool CheckServerCode (string serverCode, int ver)
        {
            RpcServiceState[] state = new RpcServiceState[]
            {
                RpcServiceState.正常,
                RpcServiceState.下线
            };
            return this._BasicDAL.IsExist(c => c.ServerCode == serverCode && c.VerNum == ver && state.Contains(c.ServiceState));
        }
        public bool CheckIsExists (long sysTypeId)
        {
            return this._BasicDAL.IsExist(c => c.SystemType == sysTypeId);
        }
        public long Add (RemoteServerConfigModel add)
        {
            add.AddTime = DateTime.Now;
            add.Id = IdentityHelper.CreateId();
            add.ServiceState = RpcServiceState.待启用;
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public int? GetServiceIndex (long sysTypeId, string mac)
        {
            return this._BasicDAL.Max<int?>(c => c.SystemType == sysTypeId && c.ServerMac == mac, c => c.ServerIndex);
        }
        public string GetName (long serverId)
        {
            return this._BasicDAL.Get(a => a.Id == serverId, a => a.ServerName);
        }
        public void SetState (long id, RpcServiceState state)
        {
            if (!this._BasicDAL.Update(a => a.ServiceState == state, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.service.state.set.error");
            }
        }

        public void SetService (long id, ServerConfigSet data)
        {
            if (!this._BasicDAL.Update(data, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.service.set.error");
            }
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.service.delete.error");
            }
        }
        public ServerConfigDatum[] Query (ServerConfigQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<ServerConfigDatum>(query.ToWhere(), paging, out count);
        }
        public RemoteServerConfigModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public Dictionary<long, int> GetContainerServerNum (long[] groupId, RpcServiceState[] states)
        {
            return this._BasicDAL.GroupBy(a => a.IsContainer && groupId.Contains(a.ContainerGroupId.Value) && states.Contains(a.ServiceState), a => a.ContainerGroupId, a => new
            {
                a.ContainerGroupId,
                count = SqlFunc.AggregateCount(a.SystemType)
            }).ToDictionary(a => a.ContainerGroupId.Value, a => a.count);
        }
        public Dictionary<long, int> GetServerNum (long[] typeId, RpcServiceState[] states)
        {
            return this._BasicDAL.GroupBy(a => typeId.Contains(a.SystemType) && states.Contains(a.ServiceState), a => a.SystemType, a => new
            {
                a.SystemType,
                count = SqlFunc.AggregateCount(a.SystemType)
            }).ToDictionary(a => a.SystemType, a => a.count);
        }
        public ServerConfigDatum[] GetServices (long[] ids)
        {
            return this._BasicDAL.Gets<ServerConfigDatum>(c => ids.Contains(c.Id));
        }
        public bool CheckIsOnline (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id, c => c.IsOnline);
        }

        public bool CheckServerPort (string mac, int serverPort)
        {
            return this._BasicDAL.IsExist(c => c.ServerMac == mac && c.ServerPort == serverPort);
        }

        public bool CheckIsExistsByGroup (long groupId)
        {
            return this._BasicDAL.IsExist(c => c.GroupId == groupId);
        }

        public bool CheckRegion (int regionId)
        {
            return this._BasicDAL.IsExist(c => c.RegionId == regionId);
        }
        public SystemTypeVerNum[] GetVerNums (long[] ids)
        {
            return this._BasicDAL.GroupBy(a => ids.Contains(a.Id), a => a.SystemType, a => new SystemTypeVerNum
            {
                VerNum = SqlFunc.AggregateMax(a.VerNum),
                SystemTypeId = a.SystemType
            });
        }
        public SystemTypeVerNum[] GetAllVerNums (long[] ids)
        {
            return this._BasicDAL.Gets(a => ids.Contains(a.Id), a => new SystemTypeVerNum
            {
                VerNum = a.VerNum,
                SystemTypeId = a.SystemType
            });
        }
        public BasicService[] GetBasics (ServerConfigQuery query)
        {
            return this._BasicDAL.Gets<BasicService>(query.ToWhere());
        }
        public Dictionary<long, string> GetNames (long[] ids)
        {
            BasicService[] list = this._BasicDAL.Gets<BasicService>(c => ids.Contains(c.Id));
            return list.ToDictionary(c => c.Id, c => c.ServerName);
        }

        public Dictionary<int, int> GetReginServerNum (int[] regionId)
        {
            return this._BasicDAL.GroupBy(a => regionId.Contains(a.RegionId), a => a.RegionId, a => new
            {
                a.RegionId,
                count = SqlFunc.AggregateCount(a.RegionId)
            }).ToDictionary(a => a.RegionId, a => a.count);
        }

        public ServerItem[] GetItems (ServerConfigQuery query)
        {
            return this._BasicDAL.Gets<ServerItem>(query.ToWhere());
        }

        public bool CheckIsExistByContainer (long containerGroupId)
        {
            return this._BasicDAL.IsExist(c => c.IsContainer && c.ContainerGroupId == containerGroupId);
        }
        public bool CheckIsExists (long systemType, string serverMac, int serverIndex)
        {
            return this._BasicDAL.IsExist(c => c.SystemType == systemType && c.ServerMac == serverMac && c.ServerIndex == serverIndex);
        }
        public int GetVerNum (long rpcMerId, long systemTypeId)
        {
            return this._BasicDAL.JoinGet<RemoteServerGroupModel, int>((a, b) => a.Id == b.ServerId && b.RpcMerId == rpcMerId && b.SystemType == systemTypeId, (a, b) => SqlFunc.AggregateMin(a.VerNum));
        }
        public void SetVerNum (long id, int verNum)
        {
            if (!this._BasicDAL.Update(a => a.VerNum == verNum, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.service.set.error");
            }
        }
    }
}
