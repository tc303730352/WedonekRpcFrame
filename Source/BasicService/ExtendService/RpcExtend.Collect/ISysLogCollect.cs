using RpcExtend.Model.DB;

namespace RpcExtend.Collect
{
    public interface ISysLogCollect
    {
        void AddLog(SystemErrorLog[] logs);
    }
}