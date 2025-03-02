
namespace WeDonekRpc.Model
{
    public class BasicResObj : BasicRes
    {
        public BasicResObj ()
        {
        }
        public BasicResObj (object res)
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
        public BasicRes ()
        {
        }
        public BasicRes (string error) : base(error)
        {
        }
        public BasicRes (T res)
        {
            this.Result = res;
        }
        public T Result
        {
            get;
            set;
        }
    }
    public class BasicResult<T> : BasicRes<T>
    {
        public BasicResult ()
        {
        }
        public BasicResult (string error, long serverId) : base(error)
        {
            this.ServerId = serverId;
        }
        public BasicResult (T res, long serverId) : base(res)
        {
            this.ServerId = serverId;
        }
        /// <summary>
        /// 远程服务节点ID
        /// </summary>
        public long ServerId { get; }
    }
}
