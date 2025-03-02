using RpcExtend.Model.DB;

namespace RpcExtend.Collect
{
    public interface IAutoRetryTaskLogCollect
    {
        void Adds (AutoRetryTaskLogModel[] adds);
    }
}