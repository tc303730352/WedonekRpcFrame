namespace RpcSync.Model
{
    public class DictateNode
    {
        public long Id
        {
            get;
            set;
        }
        public long ParentId
        {
            get;
            set;
        }

        public string Dictate
        {
            get;
            set;
        }

        public bool IsEndpoint
        {
            get;
            set;
        }
    }
}
