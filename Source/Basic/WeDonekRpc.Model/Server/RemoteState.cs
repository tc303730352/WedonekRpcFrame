namespace WeDonekRpc.Model.Server
{
    /// <summary>
    /// 远程服务状态
    /// </summary>
    public class RemoteState
    {
        /// <summary>
        /// 链接的服务节点Id
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
        /// Ping包发送量
        /// </summary>
        public int SendNum { get; set; }
        /// <summary>
        /// 当前节点链接状态
        /// </summary>
        public UsableState UsableState { get; set; }

        /// <summary>
        /// 发生Socket错误数
        /// </summary>
        public int ErrorNum { get; set; }
    }
}
