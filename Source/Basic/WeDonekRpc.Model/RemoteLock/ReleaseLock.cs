namespace WeDonekRpc.Model.RemoteLock
{
    [IRemoteConfig(SysDictate = "ReleaseLock", IsProhibitTrace = true)]
    public class ReleaseLock
    {
        public string LockId
        {
            get;
            set;
        }
        public bool IsError { get; set; }
        public string Extend
        {
            get;
            set;
        }
        public long SessionId { get; set; }

        public string Error { get; set; }
        public bool IsReset { get; set; }
    }
}
