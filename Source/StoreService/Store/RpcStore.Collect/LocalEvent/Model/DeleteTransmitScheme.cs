using RpcStore.Model.DB;
using WeDonekRpc.Client;

namespace RpcStore.Collect.LocalEvent.Model
{
    internal class DeleteTransmitScheme : EventPublic
    {
        public DeleteTransmitScheme () : base("Delete")
        {

        }
        public ServerTransmitSchemeModel Scheme { get; set; }
    }
}
