using RpcStore.Model.DB;
using WeDonekRpc.Client;

namespace RpcStore.Collect.LocalEvent.Model
{
    internal class RemoteServerGroupEvent : EventPublic
    {
        public RemoteServerGroupEvent (string name) : base(name)
        {

        }
        public RemoteServerGroupModel Source { get; set; }
    }
}
