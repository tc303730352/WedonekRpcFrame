using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using SqlSugar;

namespace RpcCentral.Model.DB
{
    [SugarTable("ServerDictateLimit")]
    public class ServerDictateLimitModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long ServerId
        {
            get;
            set;
        }
        public string Dictate
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
        /// 窗口大小（秒）
        /// </summary>
        public short LimitTime
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
        /// 令牌最大数
        /// </summary>
        public short TokenNum
        {
            get;
            set;
        }
        /// <summary>
        /// 每秒添加令牌数
        /// </summary>
        [NumValidate("rpc.token.inNum.error", 0, short.MaxValue)]
        public short TokenInNum
        {
            get;
            set;
        }
    }
}
