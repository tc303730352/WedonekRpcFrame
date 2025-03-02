using RpcStore.Collect.LocalEvent.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.Model.Group;
using RpcStore.Model.ServerGroup;
using RpcStore.RemoteModel.ServerBind.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.lmpl
{
    internal class RemoteGroupCollect : IRemoteGroupCollect
    {
        private readonly IRemoteServerGroupDAL _BasicDAL;

        public RemoteGroupCollect (IRemoteServerGroupDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public BindServerGroup[] Query (long rpcMerId, BindQueryParam query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(rpcMerId, query, paging, out count);
        }
        public RemoteGroup[] GetServers (long merId)
        {
            return this._BasicDAL.GetServers(merId);
        }
        public bool CheckIsExists (long merId)
        {
            return this._BasicDAL.CheckIsExists(merId);
        }
        public Dictionary<long, int> GetServerNum (long[] merId)
        {
            return this._BasicDAL.GetServerNum(merId);
        }
        public void Delete (RemoteServerGroupModel group)
        {
            if (group.IsHold)
            {
                throw new ErrorException("rpc.store.remote.group.not.delete");
            }
            this._BasicDAL.Delete(group.Id);
            new RemoteServerGroupEvent("Delete")
            {
                Source = group
            }.AsyncPublic();
        }
        public RemoteServerGroupModel Get (long id)
        {
            RemoteServerGroupModel param = this._BasicDAL.Get(id);
            if (param == null)
            {
                throw new ErrorException("rpc.store.remote.group.not.find");
            }
            return param;
        }
        public void Adds (long rpcMerId, RemoteServerGroup[] items)
        {
            this._BasicDAL.Adds(rpcMerId, items);
        }
        public Dictionary<long, int> GetNumBySystemType (BindGetParam param)
        {
            return this._BasicDAL.GetNumBySystemType(param);
        }
        public BindServerItem[] GetServerItems (ServerBindQueryParam query)
        {
            return this._BasicDAL.GetServerItems(query);
        }
        public long[] GetRpcMerId (long serverId)
        {
            return this._BasicDAL.GetRpcMerId(serverId);
        }
        public void SetWeight (Dictionary<long, int> weight)
        {
            this._BasicDAL.SetWeight(weight);
        }
        public long[] CheckIsBind (long rpcMerId, long[] serverId)
        {
            return this._BasicDAL.CheckIsBind(rpcMerId, serverId);
        }
        public long[] GetServerId (long rpcMerId, bool? isHold)
        {
            return this._BasicDAL.GetServerIds(rpcMerId, isHold);
        }
        public long[] GetServerId (long rpcMerId, long sysTypeId)
        {
            return this._BasicDAL.GetServerIds(sysTypeId, sysTypeId);
        }
        public long[] GetContainerGroupId (BindGetParam param)
        {
            return this._BasicDAL.GetContainerGroupId(param);
        }
    }
}
