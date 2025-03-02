namespace WeDonekRpc.Model.Tran
{
    [IRemoteConfig("SubmitTran", "sys.sync", true, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class SubmitTran
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public long TranId
        {
            get;
            set;
        }
    }
}
