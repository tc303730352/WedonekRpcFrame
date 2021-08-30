namespace RpcModel.RemoteLock
{
        public class ApplyLockRes
        {
                public RemoteLockStatus LockStatus
                {
                        get;
                        set;
                }

                public ExecResult Result
                {
                        get;
                        set;
                }
                public long LockServerId { get; set; }
                public int TimeOut { get; set; }
                public string SessionId { get; set; }
                public int OverTime { get; set; }
        }
}
