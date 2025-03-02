namespace WeDonekRpc.Model.RemoteLock
{
    [IRemoteConfig("ApplyLock", "sys.sync", Transmit = "RemoteLock", IsProhibitTrace = true)]
    public class ApplyLock
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public string LockId
        {
            get;
            set;
        }
        public int ValidTime
        {
            get;
            set;
        }
        public int LockTimeOut { get; set; }
    }
}
