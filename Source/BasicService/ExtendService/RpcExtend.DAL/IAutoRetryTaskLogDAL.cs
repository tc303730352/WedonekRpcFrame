using RpcExtend.Model.DB;

namespace RpcExtend.DAL
{
    public interface IAutoRetryTaskLogDAL
    {
        void Adds ( AutoRetryTaskLogModel[] adds );
    }
}