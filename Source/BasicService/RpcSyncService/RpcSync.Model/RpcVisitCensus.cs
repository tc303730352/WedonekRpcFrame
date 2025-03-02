namespace RpcSync.Model
{
    public class RpcVisitCensus
    {
        public long ServiceId
        {
            get;
            set;
        }
        public string Dictate
        {
            get;
            set;
        }

        public int Success
        {
            get;
            set;
        }
        public int Fail
        {
            get;
            set;
        }
        public int Concurrent { get; set; }
    }
}
