namespace WeDonekRpc.SqlSugarDbTran.Attr
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RpcDbTransaction : Attribute
    {
        public RpcDbTransaction ()
        {
        }

    }
}
