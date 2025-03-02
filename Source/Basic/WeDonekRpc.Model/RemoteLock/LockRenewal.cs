namespace WeDonekRpc.Model.RemoteLock
{
    /// <summary>
    /// 锁续期
    /// </summary>
    [IRemoteConfig(SysDictate = "LockRenewal", IsProhibitTrace = true)]
    public class LockRenewal
    {
        public string LockId
        {
            get;
            set;
        }
        public long SessionId { get; set; }
    }
}
