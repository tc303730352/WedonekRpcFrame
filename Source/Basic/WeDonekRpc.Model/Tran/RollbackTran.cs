namespace WeDonekRpc.Model.Tran
{
    [IRemoteConfig("RollbackTran", "sys.sync", false, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class RollbackTran
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public long TranId
        {
            get;
            set;
        }
    }
}
