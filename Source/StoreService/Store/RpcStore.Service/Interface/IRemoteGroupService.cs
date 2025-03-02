using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.ServerBind.Model;

namespace RpcStore.Service.Interface
{
    public interface IRemoteGroupService
    {
        ContainerGroupItem[] GetBindContainerGroup (BindGetParam param);
        BindServerGroupType[] GetGroupAndType (BindGetParam param);
        void Delete (long id);
        PagingResult<BindRemoteServer> Query (long rpcMerId, BindQueryParam query, IBasicPage paging);
        long[] CheckIsBind (long rpcMerId, long[] serverId);
        void SetBindGroup (BindServer set);
        void SetWeight (SaveWeight obj);
        BindServerItem[] GetServerItems (ServerBindQueryParam query);
        ServerBindVer[] GetBindVer(long rpcMerId, bool? isHold);
    }
}