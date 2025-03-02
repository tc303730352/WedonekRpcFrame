namespace WeDonekRpc.Model.Tran
{
    [IRemoteConfig("SetTranExtend", "sys.sync", false, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class SetTranExtend
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public long TranId
        {
            get;
            set;
        }
        public string Extend
        {
            get;
            set;
        }
    }
}
