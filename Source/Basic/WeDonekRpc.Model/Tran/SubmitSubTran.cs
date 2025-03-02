using System;

namespace WeDonekRpc.Model.Tran
{
    [IRemoteConfig("SubmitSubTran", "sys.sync", false, true, Transmit = "RpcTran", IsProhibitTrace = true)]
    public class SubmitSubTran
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public Guid TranId
        {
            get;
            set;
        }
    }
}
