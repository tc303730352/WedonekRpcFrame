using WeDonekRpc.Client.Interface;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Event
{
    internal class RpcNodeEvent : IRpcApiService
    {
        private INodeLoadService _Service;

        public RpcNodeEvent(INodeLoadService service)
        {
            this._Service = service;
        }

        public void RefreshDictateNode()
        {
            _Service.LoadDictateNode();
        }
        public void RefreshServerNode()
        {
            _Service.LoadServerNode();
        }
    }
}
