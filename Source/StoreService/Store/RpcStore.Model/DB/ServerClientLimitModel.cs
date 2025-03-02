using SqlSugar;
using WeDonekRpc.Model;

namespace RpcStore.Model.DB
{
    /// <summary>
    /// 服务节点客户端限流配置
    /// </summary>
    [SugarTable("ServerClientLimit")]
    public class ServerClientLimitModel
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 限制类型
        /// </summary>
        public ServerLimitType LimitType
        {
            get;
            set;
        }
        /// <summary>
        /// 限制发送数量
        /// </summary>
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 限定窗口时间
        /// </summary>
        public short LimitTime
        {
            get;
            set;
        }
        /// <summary>
        /// 限定令牌数
        /// </summary>
        public short TokenNum
        {
            get;
            set;
        }
        /// <summary>
        /// 每秒写入令牌数量
        /// </summary>
        public short TokenInNum
        {
            get;
            set;
        }
    }
}
