using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class BindServer
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [NumValidate("rpc.store.mer.id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [NumValidate("rpc.store.server.id.error", 1)]
        public long[] ServerId
        {
            get;
            set;
        }
    }
}
