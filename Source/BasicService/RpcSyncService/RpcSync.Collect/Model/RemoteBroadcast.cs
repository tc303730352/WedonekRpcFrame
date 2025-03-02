namespace RpcSync.Collect.Model
{
    public class RemoteBroadcast
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
