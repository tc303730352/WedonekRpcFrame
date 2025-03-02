namespace WeDonekRpc.Model.RemoteLock
{
    [IRemoteConfig(SysDictate = "Rpc_Unlock", IsProhibitTrace = true)]
    public class Unlock
    {
        public string LockId
        {
            get;
            set;
        }
        public ExecResult Result
        {
            get;
            set;
        }
        public int OverTime
        {
            get;
            set;
        }
    }
}
