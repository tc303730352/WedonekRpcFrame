namespace WeDonekRpc.Model.Tran
{
    /// <summary>
    /// 获取事务结果
    /// </summary>
    [IRemoteConfig("GetTranResult", "sys.sync", true, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class GetTranResult
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public long TranId
        {
            get;
            set;
        }
    }
}
