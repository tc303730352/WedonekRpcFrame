using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.SignalState.Model
{
    public class ServerSignalState
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是容器
        /// </summary>
        public bool IsContainer
        {
            get;
            set;
        }
        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState ServiceState
        {
            get;
            set;
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline
        {
            get;
            set;
        }
        /// <summary>
        /// 链接的服务节点
        /// </summary>
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
