using RpcExtend.Model.DB;

namespace RpcExtend.DAL
{
    public interface ISystemLogDAL
    {
        void Adds(SystemErrorLog[] logs);
    }
}