namespace WeDonekRpc.Model.Tran
{
    [IRemoteConfig("ApplyTran", "sys.sync", true, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class ApplyTran
    {
        /// <summary>
        /// 事务Id
        /// </summary>
        [TransmitColumn(TransmitType.ZoneIndex)]
        public long TranId
        {
            get;
            set;
        }
        /// <summary>
        /// 事务名
        /// </summary>
        public string TranName
        {
            get;
            set;
        }

        /// <summary>
        /// 提交的数据
        /// </summary>
        public string SubmitJson
        {
            get;
            set;
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int TimeOut
        {
            get;
            set;
        }
        public RpcTranMode TranMode { get; set; }
    }
}
