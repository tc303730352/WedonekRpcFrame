using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.ServerBind;
using RpcStore.RemoteModel.ServerBind.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Service.Event
{
    internal class RempteGroupEvent : IRpcApiService
    {
        private readonly IRemoteGroupService _Service;

        public RempteGroupEvent (IRemoteGroupService service)
        {
            this._Service = service;
        }
        public ServerBindVer[] GetServerBindVer (GetServerBindVer obj)
        {
            return this._Service.GetBindVer(obj.RpcMerId, obj.IsHold);
        }
        public BindServerItem[] GetServerBindItems (GetServerBindItems obj)
        {
            return this._Service.GetServerItems(obj.Query);
        }
        public void DeleteServerBind (DeleteServerBind obj)
        {
            this._Service.Delete(obj.Id);
        }
        public ContainerGroupItem[] GetBindContainerGroup (GetBindContainerGroup obj)
        {
            return this._Service.GetBindContainerGroup(obj.Param);
        }
        public BindServerGroupType[] GetBindServerGroupType (GetBindServerGroupType obj)
        {
            return this._Service.GetGroupAndType(obj.Param);
        }
        public long[] CheckIsBindServer (CheckIsBindServer obj)
        {
            return this._Service.CheckIsBind(obj.RpcMerId, obj.ServerId);
        }
        public PagingResult<BindRemoteServer> QueryBindServer (QueryBindServer query)
        {
            return this._Service.Query(query.RpcMerId, query.Query, query.ToBasicPage());
        }

        public void SetBindServer (SetBindServer set)
        {
            this._Service.SetBindGroup(set.Bind);
        }

        public void SaveBindWeight (SaveBindWeight set)
        {
            this._Service.SetWeight(set.Weight);
        }
    }
}
