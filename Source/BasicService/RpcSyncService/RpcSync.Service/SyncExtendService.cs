using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace RpcSync.Service
{
    [IocName("SyncExtendService")]
    internal class SyncExtendService : IRpcExtendService
    {
        private readonly INodeRefreshService _Node;
        public SyncExtendService (INodeRefreshService node)
        {
            this._Node = node;
        }

        public void Load (IRpcService service)
        {
            service.InitComplating += this.Service_InitComplating;
        }

        private void Service_InitComplating ()
        {
            this._Node.Init();
        }
    }
}
