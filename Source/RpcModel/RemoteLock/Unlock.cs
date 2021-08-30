namespace RpcModel.RemoteLock
{
        [IRemoteConfig(SysDictate = "Rpc_Unlock")]
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
