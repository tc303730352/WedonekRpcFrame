using RpcStore.Model.DB;
using WeDonekRpc.Client;

namespace RpcStore.Collect.LocalEvent.Model
{
    internal class RpcMerEvent : EventPublic
    {
        public RpcMerEvent (string name) : base(name)
        {

        }
        public RpcMerModel RpcMer { get; set; }
    }
}
