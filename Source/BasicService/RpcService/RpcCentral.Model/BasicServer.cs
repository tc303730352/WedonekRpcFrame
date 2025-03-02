namespace RpcCentral.Model
{
    /// <summary>
    /// 服务节点配置
    /// </summary>
    public class BasicServer
    {
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务编号
        /// </summary>
        public string ServerCode { get; set; }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int VerNum { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// 最后离线日期
        /// </summary>
        public DateTime LastOffliceDate { get; set; }

        public long HoldRpcMerId { get; set; }
    }
}
