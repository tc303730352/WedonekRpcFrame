namespace RpcModel.RemoteLock
{
        [IRemoteConfig(SysDictate = "ReleaseLock")]
        public class ReleaseLock
        {
                public string LockId
                {
                        get;
                        set;
                }
                public string Extend
                {
                        get;
                        set;
                }
                public string SessionId { get; set; }
                public bool IsReset { get; set; }
        }
}
