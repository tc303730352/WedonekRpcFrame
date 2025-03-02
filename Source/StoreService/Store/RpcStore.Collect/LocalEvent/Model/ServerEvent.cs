using RpcStore.Model.DB;
using WeDonekRpc.Client;

namespace RpcStore.Collect.LocalEvent.Model
{
    internal class ServerEvent : EventPublic
    {
        public ServerEvent (string name) : base(name)
        {

        }
        public RemoteServerConfigModel Server { get; set; }
    }

}
