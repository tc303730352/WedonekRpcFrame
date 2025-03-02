using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.ServerBind;
using RpcStore.RemoteModel.ServerBind.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Services
{
    internal class ServerBindService : IServerBindService
    {
        public long[] CheckIsBind (long rpcMerId, long[] serverId)
        {
            return new CheckIsBindServer
            {
                RpcMerId = rpcMerId,
                ServerId = serverId
            }.Send();
        }
        public ServerBindVer[] GetServerBindVer (long rpcMerId, bool? isHold)
        {
            return new GetServerBindVer
            {
                IsHold = isHold,
                RpcMerId = rpcMerId
            }.Send();
        }
        public ContainerGroupItem[] GetContainerGroup (BindGetParam param)
        {
            return new GetBindContainerGroup
            {
                Param = param
            }.Send();
        }
        public BindServerGroupType[] GetGroupAndType (BindGetParam param)
        {
            return new GetBindServerGroupType
            {
                Param = param
            }.Send();
        }
        public void DeleteServerBind (long id)
        {
            new DeleteServerBind
            {
                Id = id,
            }.Send();
        }
        public BindRemoteServer[] Query (long rpcMerId, BindQueryParam query, IBasicPage paging, out int count)
        {
            return new QueryBindServer
            {
                RpcMerId = rpcMerId,
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetBindServer (BindServer bind)
        {
            new SetBindServer
            {
                Bind = bind,
            }.Send();
        }

        public void SaveWeight (SaveWeight weight)
        {
            new SaveBindWeight
            {
                Weight = weight,
            }.Send();
        }

        public BindServerItem[] GetItems (ServerBindQueryParam query)
        {
            return new GetServerBindItems
            {
                Query = query
            }.Send();
        }
    }
}
