namespace RpcSync.Model
{
    [Serializable]
    public class ErrorDatum
    {
        public long ErrorId
        {
            get;
            set;
        }
        public bool IsPerfect
        {
            get;
            set;
        }
    }
}
