using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
namespace RpcStore.RemoteModel.DictateLimit.Model
{
    public class DictateLimitQuery
    {
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [NumValidate("rpc.store.server.id.null", 1)]
        public long ServerId { get; set; }

        /// <summary>
        /// 指令
        /// </summary>
        public string Dictate
        {
            get;
            set;
        }

        /// <summary>
        /// 限制类型
        /// </summary>
        public ServerLimitType? LimitType
        {
            get;
            set;
        }

    }
}
