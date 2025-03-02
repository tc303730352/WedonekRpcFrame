using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Model.Tran
{
    [IRemoteConfig("AddTranLog", "sys.sync", true, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class AddTranLog
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public long TranId
        {
            get;
            set;
        }
        public TranLogDatum TranLog
        {
            get;
            set;
        }
    }
}
