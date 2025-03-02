using RpcStore.RemoteModel.ServerVer;
using RpcStore.RemoteModel.ServerVer.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Service.Event
{
    internal class RpcMerServerVerEvent : IRpcApiService
    {
        private readonly IRpcMerServerVerService _Service;

        public RpcMerServerVerEvent (IRpcMerServerVerService service)
        {
            this._Service = service;
        }

        public ServerVerInfo[] GetServerVers (GetServerVers obj)
        {
            return this._Service.GetVerList(obj.RpcMerId);
        }

        public void SetServerVer (SetServerVer obj)
        {
            this._Service.SetCurrentVer(obj.RpcMerId, obj.SystemTypeId, obj.VerNum);
        }
    }
}
