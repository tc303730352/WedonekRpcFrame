using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.DictateLimit.Model
{
    public class DictateLimitAdd : DictateLimitSet
    {
        /// <summary>
        /// 限流指令
        /// </summary>
        [NullValidate("rpc.store.dictate.null")]
        [LenValidate("rpc.store.dictate.len", 1, 50)]
        public string Dictate
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        [NumValidate("rpc.store.server.id.error", 1)]
        public long ServerId
        {
            get;
            set;
        }
    }
}
