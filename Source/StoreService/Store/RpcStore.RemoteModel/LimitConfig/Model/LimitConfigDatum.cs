using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.LimitConfig.Model
{
    public class LimitConfigDatum
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        [NumValidate("rpc.store.service.Id.error", 1)]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
        /// <summary>
        /// 限流类型
        /// </summary>
        public ServerLimitType LimitType
        {
            get;
            set;
        }
        /// <summary>
        /// 最大流量
        /// </summary>
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 窗口大小
        /// </summary>
        public short LimitTime
        {
            get;
            set;
        }

        /// <summary>
        /// 操过限制的时候是否启用桶
        /// </summary>
        public bool IsEnableBucket
        {
            get;
            set;
        }
        /// <summary>
        /// 桶大小
        /// </summary>
        public short BucketSize
        {
            get;
            set;
        }
        /// <summary>
        /// 桶溢出速度
        /// </summary>
        public short BucketOutNum
        {
            get;
            set;
        }
        /// <summary>
        /// 令牌总量
        /// </summary>
        public short TokenNum { get; set; }
        /// <summary>
        /// 令牌每秒新增数
        /// </summary>
        public short TokenInNum { get; set; }
    }
}
