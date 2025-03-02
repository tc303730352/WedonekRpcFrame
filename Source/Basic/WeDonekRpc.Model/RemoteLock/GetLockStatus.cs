namespace WeDonekRpc.Model.RemoteLock
{
    [IRemoteConfig(SysDictate = "GetLockStatus", IsProhibitTrace = true)]
    public class GetLockStatus
    {
        public string LockId
        {
            get;
            set;
        }
        public long SessionId { get; set; }
    }
}
