namespace RpcStore.RemoteModel.IpBlack.Model
{
    public class IpBlack
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 类别
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// Ip类型
        /// </summary>
        public IpType IpType { get; set; }
        /// <summary>
        /// Ip地址段
        /// </summary>
        public string Ip { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
