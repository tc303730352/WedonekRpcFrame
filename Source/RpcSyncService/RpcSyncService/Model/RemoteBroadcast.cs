namespace RpcSyncService.Model
{
        internal class RemoteBroadcast
        {
                public BroadcastBody Msg
                {
                        get;
                        set;
                }

                public long ServerId
                {
                        get;
                        set;
                }

                public string TypeVal
                {
                        get;
                        set;
                }
        }
}
