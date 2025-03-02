using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerGroup.Model;
using RpcStore.RemoteModel.ServerType.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ServerGroupService : IServerGroupService
    {
        private readonly IServerGroupCollect _Group;
        private readonly IServerTypeCollect _ServerType;
        public ServerGroupService (IServerGroupCollect group, IServerTypeCollect serverType)
        {
            this._Group = group;
            this._ServerType = serverType;
        }
        public long AddGroup (ServerGroupAdd add)
        {
            return this._Group.AddGroup(add);
        }
        public ServerGroupList[] GetList (RpcServerType? serverType)
        {
            ServerGroupModel[] groups = this._Group.GetGroups();
            if (groups.IsNull())
            {
                return new ServerGroupList[0];
            }
            ServerType[] types = this._ServerType.GetAll();
            if (serverType.HasValue)
            {
                types = types.FindAll(c => c.ServiceType == serverType.Value);
                if (types.IsNull())
                {
                    return new ServerGroupList[0];
                }
                groups = groups.FindAll(c => types.IsExists(a => a.GroupId == c.Id));
            }
            return groups.ConvertMap<ServerGroupModel, ServerGroupList>((a, b) =>
            {
                b.ServerType = types.ConvertMap<ServerType, BasicServerType>(c => c.GroupId == a.Id);
                return b;
            });
        }
        public void CheckIsRepeat (string typeVal)
        {
            this._Group.CheckIsRepeat(typeVal);
        }

        public void Delete (long id)
        {
            ServerGroupModel group = this._Group.GetGroup(id);
            if (this._ServerType.CheckIsExists(group.Id))
            {
                throw new ErrorException("rpc.store.server.group.not.can.delete");
            }
            this._Group.Delete(group);
        }

        public ServerGroupDatum GetGroup (long id)
        {
            ServerGroupModel group = this._Group.GetGroup(id);
            return group.ConvertMap<ServerGroupModel, ServerGroupDatum>();
        }

        public ServerGroupItem[] GetGroups ()
        {
            ServerGroupModel[] groups = this._Group.GetGroups();
            if (groups.IsNull())
            {
                return new ServerGroupItem[0];
            }
            ServerGroupItem[] items= groups.ConvertMap<ServerGroupModel, ServerGroupItem>();
            Dictionary<long,int> nums= this._ServerType.GetNum(groups.ConvertAll(c => c.Id));
            items.ForEach(c =>
            {
                c.SystemTypeNum = nums.GetValueOrDefault(c.Id);
            });
            return items;
        }

        public void SetGroup (long id, string name)
        {
            ServerGroupModel group = this._Group.GetGroup(id);
            this._Group.SetGroup(group, name);
        }
    }
}
