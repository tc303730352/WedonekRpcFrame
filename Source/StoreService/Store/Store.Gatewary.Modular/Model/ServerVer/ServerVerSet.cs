using WeDonekRpc.Helper.Validate;

namespace Store.Gatewary.Modular.Model.ServerVer
{
    public class ServerVerSet
    {
        [NumValidate("rpc.store.mer.rpcMerId.error", 1)]
        public long RpcMerId { get; set; }
        [NumValidate("rpc.store.system.type.id.error", 1)]
        public long SystemTypeId { get; set; }
        [NumValidate("rpc.store.ver.num.error", 0)]
        public int VerNum { get; set; }
    }
}
