using RpcStore.Model.DB;
using RpcStore.Model.Group;
using RpcStore.Model.ServerGroup;
using RpcStore.RemoteModel.ServerBind.Model;
using SqlSugar;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class RemoteServerGroupDAL : IRemoteServerGroupDAL
    {
        private readonly IRepository<RemoteServerGroupModel> _BasicDAL;
        public RemoteServerGroupDAL (IRepository<RemoteServerGroupModel> dal)
        {
            this._BasicDAL = dal;
        }
        public void Clear (long serverId)
        {
            _ = this._BasicDAL.Delete(a => a.ServerId == serverId);
        }
        public long[] GetServerId (long rpcMerId, long sysTypeId)
        {
            return this._BasicDAL.Gets(a => a.RpcMerId == rpcMerId && a.SystemType == sysTypeId, a => a.ServerId);
        }
        public long[] GetServerIds (long rpcMerId, bool? isHold)
        {
            if (isHold.HasValue)
            {
                return this._BasicDAL.Gets<long>(c => c.RpcMerId == rpcMerId && c.IsHold == isHold.Value, a => a.ServerId);
            }
            return this._BasicDAL.Gets<long>(c => c.RpcMerId == rpcMerId, a => a.ServerId);
        }
        public RemoteGroup[] GetServers (long merId)
        {
            return this._BasicDAL.Gets<RemoteGroup>(c => c.RpcMerId == merId);
        }
        public Dictionary<long, int> GetServerNum (long[] merId)
        {
            var list = this._BasicDAL.GroupBy(c => merId.Contains(c.RpcMerId), c => c.RpcMerId, c => new
            {
                c.RpcMerId,
                count = SqlFunc.AggregateCount(c.RpcMerId)
            });
            return list.ToDictionary(c => c.RpcMerId, c => c.count);
        }

        public bool CheckIsExists (long merId)
        {
            return this._BasicDAL.IsExist(c => c.RpcMerId == merId);
        }
        public RemoteServerGroupModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }

        public bool CheckIsExists (long merId, long serverId)
        {
            return this._BasicDAL.IsExist(c => c.RpcMerId == merId && c.ServerId == serverId);
        }
        public long[] GetIds (long serverId)
        {
            return this._BasicDAL.Gets(c => c.ServerId == serverId, c => c.Id);
        }
        public long[] GetRpcMerId (long serverId)
        {
            return this._BasicDAL.Gets(c => c.ServerId == serverId, c => c.RpcMerId);
        }
        public void Delete (long[] ids)
        {
            if (!this._BasicDAL.Delete(c => ids.Contains(c.Id)))
            {
                throw new ErrorException("rpc.store.server.group.delete.error");
            }
        }

        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(c => c.Id == id))
            {
                throw new ErrorException("rpc.store.server.group.delete.error");
            }
        }
        public void SetWeight (Dictionary<long, int> weight)
        {
            ISqlQueue<RemoteServerGroupModel> queue = this._BasicDAL.BeginQueue();
            weight.ForEach((id, num) =>
            {
                queue.UpdateOneColumn(a => a.Weight == num, a => a.Id == id);
            });
            if (queue.Submit() <= 0)
            {
                throw new ErrorException("rpc.store.server.group.weight.set.error");
            }
        }
        public void AddHold (long rpcMerId, RemoteServerGroup item)
        {
            this._BasicDAL.Insert(new RemoteServerGroupModel
            {
                Id = IdentityHelper.CreateId(),
                RpcMerId = rpcMerId,
                ServerId = item.ServerId,
                SystemType = item.SystemType,
                ServiceType = item.ServiceType,
                RegionId = item.RegionId,
                TypeVal = item.TypeVal,
                Weight = 1,
                IsHold = true
            });
        }
        public void Adds (long rpcMerId, RemoteServerGroup[] items)
        {
            RemoteServerGroupModel[] adds = items.ConvertMap<RemoteServerGroup, RemoteServerGroupModel>((a, b) =>
            {
                b.Id = IdentityHelper.CreateId();
                b.RpcMerId = rpcMerId;
                b.Weight = 1;
                b.IsHold = false;
                return b;
            });
            this._BasicDAL.Insert(adds);
        }

        public RemoteGroup[] GetServers (long rpcMerId, long[] serverId)
        {
            return this._BasicDAL.Gets<RemoteGroup>(c => c.RpcMerId == rpcMerId && serverId.Contains(c.ServerId));
        }
        public BindServerGroup[] Query (long rpcMerId, BindQueryParam query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("ServerId", true);
            return this._BasicDAL.JoinQuery<RemoteServerConfigModel, BindServerGroup>((a, b) => b.Id == a.ServerId, query.ToWhere(rpcMerId), (a, b) => new BindServerGroup
            {
                BindId = a.Id,
                ServerId = a.ServerId,
                RegionId = b.RegionId,
                SystemType = b.SystemType,
                Weight = a.Weight,
                ServerName = b.ServerName,
                GroupId = b.GroupId,
                IsContainer = b.IsContainer,
                ServiceState = b.ServiceState,
                IsOnline = b.IsOnline,
                ServiceType = b.ServiceType,
                HoldRpcMerId = b.HoldRpcMerId,
                ContainerId = b.ContainerId,
                ServerMac = b.ServerMac,
                ServerIp = b.ServerIp,
                ServerPort = b.ServerPort,
                IsHold = a.IsHold
            }, paging, out count);
        }
        public long[] CheckIsBind (long rpcMerId, long[] serverId)
        {
            return this._BasicDAL.Gets(c => c.RpcMerId == rpcMerId && serverId.Contains(c.ServerId), a => a.ServerId);
        }

        public Dictionary<long, int> GetNumBySystemType (BindGetParam param)
        {
            return this._BasicDAL.Gets(param.ToBindGetWhere(), a => a.SystemType).GroupBy(a => a).ToDictionary(a => a.Key, a => a.Count());
        }
        public long[] GetContainerGroupId (BindGetParam param)
        {
            return this._BasicDAL.JoinGroupBy<RemoteServerConfigModel, long>((a, b) => a.ServerId == b.Id && b.IsContainer, param.ToWhere(), (a, b) => new
            {
                b.ContainerGroupId
            }, (a, b) => b.ContainerGroupId.Value);
        }
        public BindServerItem[] GetServerItems (ServerBindQueryParam query)
        {
            return this._BasicDAL.Join((a, b) => b.Id == a.ServerId, query.ToWhere(), (a, b) => new BindServerItem
            {
                ServerId = a.ServerId,
                ServerName = b.ServerName,
                ServerCode = b.ServerCode,
                ServiceState = b.ServiceState,
                ServerIp = b.ServerIp,
                ServerMac = b.ServerMac,
                ServerPort = b.ServerPort,
                IsContainer = b.IsContainer,
            });
        }

        public RemoteServerGroupModel Find (long rpcMerId, long serverId)
        {
            return this._BasicDAL.Get(c => c.RpcMerId == rpcMerId && c.ServerId == serverId);
        }

        public void SetIsHold (long id, bool isHold)
        {
            if (!this._BasicDAL.Update(a => a.IsHold == isHold, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.server.group.set.fail");
            }
        }
        public long[] GetServerIds (long rpcMerId, long systemTypeId)
        {
            return this._BasicDAL.Gets(a => a.RpcMerId == rpcMerId && a.ServerId == systemTypeId, a => a.ServerId);
        }
    }
}
