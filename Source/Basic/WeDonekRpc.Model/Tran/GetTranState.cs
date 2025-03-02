namespace WeDonekRpc.Model.Tran
{
    /// <summary>
    /// 获取事务状态
    /// </summary>
    [IRemoteConfig("GetTranState", "sys.sync", true, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class GetTranState
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
    }
}
