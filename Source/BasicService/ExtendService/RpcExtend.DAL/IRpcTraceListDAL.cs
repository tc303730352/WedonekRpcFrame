using RpcExtend.Model.DB;

namespace RpcExtend.DAL
{
    public interface IRpcTraceListDAL
    {
        void Adds(RpcTrace[] traces);
    }
}