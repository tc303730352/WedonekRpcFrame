using RpcExtend.Model.DB;

namespace RpcExtend.DAL
{
    public interface IRpcTraceLogDAL
    {
        void Adds(RpcTraceLog[] logs);
    }
}