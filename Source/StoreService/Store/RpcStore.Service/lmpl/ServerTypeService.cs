using WeDonekRpc.Client;

using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerType.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ServerTypeService : IServerTypeService
    {
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerCollect _Server;

        private readonly IServerGroupCollect _ServerGroup;
        public ServerTypeService (IServerTypeCollect type,
            IServerCollect server,
            IServerGroupCollect serverGroup)
        {
            this._ServerGroup = serverGroup;
            this._Server = server;
            this._ServerType = type;
        }
        public long Add (ServerTypeAdd add)
        {
            return this._ServerType.Add(add);
        }

        public void CheckIsRepeat (string typeVal)
        {
            this._ServerType.CheckIsRepeat(typeVal);
        }

        public void Delete (long id)
        {
            if (this._Server.CheckIsExists(id))
            {
                throw new ErrorException("rpc.store.server.type.noallow.drop");
            }
            RemoteServerTypeModel type = this._ServerType.Get(id);
            this._ServerType.Delete(type);
        }

        public ServerType Get (long id)
        {
            RemoteServerTypeModel type = this._ServerType.Get(id);
            return type.ConvertMap<RemoteServerTypeModel, ServerType>();
        }

        public string GetName(string typeVal)
        {
            return this._ServerType.GetName(typeVal);
        }

        public ServerType[] Gets (long groupId)
        {
            return this._ServerType.Gets(groupId);
        }

        public PagingResult<ServerTypeDatum> Query (ServerTypeQuery query, IBasicPage paging)
        {
            ServerType[] types = this._ServerType.Query(query, paging, out int count);
            if (types.IsNull())
            {
                return new PagingResult<ServerTypeDatum>();
            }
            Dictionary<long,string> groups = this._ServerGroup.GetGroupName(types.Distinct(a => a.GroupId));
            Dictionary<long, int> serverNum = this._Server.GetServerNum(types.ConvertAll(a => a.Id), new RpcServiceState[]
            {
                RpcServiceState.正常,
                RpcServiceState.下线,
                RpcServiceState.待启用
            });
            ServerTypeDatum[] list = types.ConvertMap<ServerType, ServerTypeDatum>((a, b) =>
            {
                b.GroupName = groups.GetValueOrDefault(a.GroupId);
                b.ServerNum = serverNum.GetValueOrDefault(a.Id);
                return b;
            });
            return new PagingResult<ServerTypeDatum>(list, count);
        }

        public void Set (long id, ServerTypeSet param)
        {
            RemoteServerTypeModel type = this._ServerType.Get(id);
            _ = this._ServerType.Set(type, param);
        }
    }
}
