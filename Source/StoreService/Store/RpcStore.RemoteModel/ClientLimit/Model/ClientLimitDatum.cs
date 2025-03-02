using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ClientLimit.Model
{
    /// <summary>
    /// 限流配置信息
    /// </summary>
    public class ClientLimitDatum
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
        /// 节点Id
        /// </summary>
        [NumValidate("rpc.store.server.id.error", 1)]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 限制类型
        /// </summary>
        [EnumValidate("rpc.store.limit.type.error", typeof(ServerLimitType), 4)]
        [EntrustValidate("_Check")]
        public ServerLimitType LimitType
        {
            get;
            set;
        }

        /// <summary>
        /// 限制发送数量
        /// </summary>
        [NumValidate("rpc.store.limit.num.error", 0, int.MaxValue)]
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 限定窗口时间
        /// </summary>
        [NumValidate("rpc.store.limit.time.error", 0, short.MaxValue)]
        public short LimitTime
        {
            get;
            set;
        }
        /// <summary>
        /// 限定令牌数
        /// </summary>
        [NumValidate("rpc.store.token.num.error", 0, short.MaxValue)]
        public short TokenNum
        {
            get;
            set;
        }
        /// <summary>
        /// 每秒写入令牌数量
        /// </summary>
        [NumValidate("rpc.store.token.inNum.error", 0, short.MaxValue)]
        public short TokenInNum
        {
            get;
            set;
        }
        private static bool _Check (ClientLimitDatum obj, out string error)
        {
            if (obj.LimitType == ServerLimitType.不启用)
            {
                error = null;
                return true;
            }
            else if (obj.LimitType == ServerLimitType.令牌桶)
            {
                if (obj.TokenNum == 0)
                {
                    error = "rpc.store.token.num.error";
                    return false;
                }
                else if (obj.TokenInNum == 0)
                {
                    error = "rpc.store.token.inNum.error";
                    return false;
                }
            }
            else if (obj.LimitNum == 0)
            {
                error = "rpc.store.limit.num.error";
                return false;
            }
            else if (obj.LimitTime == 0)
            {
                error = "rpc.store.limit.time.error";
                return false;
            }
            error = null;
            return true;
        }
    }
}
