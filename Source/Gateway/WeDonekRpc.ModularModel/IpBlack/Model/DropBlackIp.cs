namespace WeDonekRpc.ModularModel.IpBlack.Model
{
    /// <summary>
    /// 删除日志
    /// </summary>
    public class DropBlackIp
    {
        /// <summary>
        /// 黑名单Ip
        /// </summary>
        public long Ip
        {
            get;
            set;
        }
        /// <summary>
        /// 结束位IP
        /// </summary>
        public long? EndIp { get; set; }
    }
}
