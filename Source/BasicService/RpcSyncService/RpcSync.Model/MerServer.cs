namespace RpcSync.Model
{
    [Serializable]
    public class MerServer
    {
        public long ServerId
        {
            get;
            set;
        }
        public long SystemType
        {
            get;
            set;
        }
        public int RegionId
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
