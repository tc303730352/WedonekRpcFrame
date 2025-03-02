namespace RpcStore.RemoteModel.IpBlack.Model
{
    /// <summary>
    /// 黑名单
    /// </summary>
    public class IpBlackAdd
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// Ip类型
        /// </summary>
        public IpType IpType { get; set; }

        public string Ip6 { get; set; }
        /// <summary>
        /// 起始IP
        /// </summary>
        public long Ip
        {
            get;
            set;
        }
        /// <summary>
        /// 截止IP
        /// </summary>
        public long? EndIp { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
