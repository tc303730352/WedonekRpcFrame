using WeDonekRpc.Model;
using SqlSugar;

namespace RpcStore.Model.DB
{
    /// <summary>
    /// 服务节点链接状态
    /// </summary>
    [SugarTable("ServerSignalState")]
    public class ServerSignalStateModel
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        [SugarColumn(IsPrimaryKey =true)]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 链接的服务节点
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long RemoteId
        {
            get;
            set;
        }
        /// <summary>
        /// 链接数
        /// </summary>
        public int ConNum
        {
            get;
            set;
        }
        /// <summary>
        /// 平均响应时间
        /// </summary>
        public int AvgTime
        {
            get;
            set;
        }
        /// <summary>
        /// 发送量
        /// </summary>
        public int SendNum
        {
            get;
            set;
        }
        /// <summary>
        /// 错误量
        /// </summary>
        public int ErrorNum
        {
            get;
            set;
        }
        /// <summary>
        /// 可用状态
        /// </summary>
        public UsableState UsableState
        {
            get;
            set;
        }
        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime SyncTime
        {
            get;
            set;
        }
    }
}
