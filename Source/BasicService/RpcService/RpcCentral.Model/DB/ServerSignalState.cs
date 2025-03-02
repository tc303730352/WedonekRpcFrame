using WeDonekRpc.Model;
using SqlSugar;

namespace RpcCentral.Model.DB
{
    /// <summary>
    /// 服务节点状态
    /// </summary>
    [SugarTable("ServerSignalState")]
    public class ServerSignalState : IEquatable<ServerSignalState>
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 远程节点Id
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
        /// 发送数
        /// </summary>
        public int SendNum
        {
            get;
            set;
        }

        /// <summary>
        /// 错误数
        /// </summary>
        public int ErrorNum { get; set; }
        /// <summary>
        /// 可用状态
        /// </summary>
        public UsableState UsableState { get; set; }

        public DateTime SyncTime { get; set; }
        public override bool Equals ( object obj )
        {
            if ( obj is ServerSignalState i )
            {
                return i.ServerId == this.ServerId && i.RemoteId == this.RemoteId;
            }
            return false;
        }

        public bool Equals ( ServerSignalState other )
        {
            if ( other == null )
            {
                return false;
            }
            return other.ServerId == this.ServerId && other.RemoteId == this.RemoteId;
        }

        public override int GetHashCode ()
        {
            return string.Concat(this.ServerId, "_", this.RemoteId).GetHashCode();
        }
    }
}
