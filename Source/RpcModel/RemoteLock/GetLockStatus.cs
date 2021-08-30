namespace RpcModel.RemoteLock
{
        [IRemoteConfig(SysDictate = "GetLockStatus")]
        public class GetLockStatus
        {
                public string LockId
                {
                        get;
                        set;
                }
                public string SessionId { get; set; }
        }
}
