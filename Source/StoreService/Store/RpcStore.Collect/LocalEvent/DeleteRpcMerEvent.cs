using RpcStore.DAL;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Collect.LocalEvent
{
    [LocalEventName("Delete")]
    internal class DeleteRpcMerEvent : IEventHandler<Model.RpcMerEvent>
    {
        private readonly IServerTransmitSchemeDAL _SchemeDAL;
        public DeleteRpcMerEvent (IServerTransmitSchemeDAL schemeDAL)
        {
            this._SchemeDAL = schemeDAL;
        }

        public void HandleEvent (Model.RpcMerEvent data, string eventName)
        {
            this._SchemeDAL.Clear(data.RpcMer.Id);
        }
    }
}
