using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace RpcSync.Service.Broadcast
{
    [IocName("BroadcastExtendService")]
    internal class BroadcastExtendService : IRpcExtendService
    {
        private readonly IDeadQueueService _DeadQueue;

        public BroadcastExtendService (IDeadQueueService deadQueue)
        {
            this._DeadQueue = deadQueue;
        }

        public void Load (IRpcService service)
        {
            service.StartUpComplate += this.Service_StartUpComplate;
        }

        private void Service_StartUpComplate ()
        {
            this._DeadQueue.Init();
        }
    }
}
