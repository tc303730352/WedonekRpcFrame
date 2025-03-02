namespace RpcStore.RemoteModel.IpBlack.Model
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class IpBlackQuery
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// Ip类型
        /// </summary>
        public IpType? IpType
        {
            get;
            set;
        }

        /// <summary>
        /// Ip6
        /// </summary>
        public string Ip
        {
            get;
            set;
        }
    }
}
