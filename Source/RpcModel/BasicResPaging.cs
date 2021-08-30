namespace RpcModel
{
        public class BasicResPaging<T> : BasicRes
        {
                public BasicResPaging()
                {
                }

                public T[] DataList
                {
                        get;
                        set;
                }
                public long Count
                {
                        get;
                        set;
                }
        }
}
