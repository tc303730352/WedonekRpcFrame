
namespace RpcModel
{
        public class BasicResObj : BasicRes
        {
                public BasicResObj()
                {
                }
                public BasicResObj(object res)
                {
                        this.Result = res;
                }
                public object Result
                {
                        get;
                        set;
                }
        }
        public class BasicRes<T> : BasicRes
        {
                public BasicRes()
                {
                }
                public BasicRes(T res)
                {
                        this.Result = res;
                }
                public T Result
                {
                        get;
                        set;
                }
        }
}
