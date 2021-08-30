namespace RpcModel.RemoteLock
{
        [IRemoteConfig("ApplyLock", "sys.sync")]
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
