using RpcExtend.DAL;
using RpcExtend.Model.DB;

namespace RpcExtend.Collect.Collect
{
    internal class AutoRetryTaskLogCollect : IAutoRetryTaskLogCollect
    {
        private readonly IAutoRetryTaskLogDAL _LogDAL;

        public AutoRetryTaskLogCollect (IAutoRetryTaskLogDAL logDAL)
        {
            this._LogDAL = logDAL;
        }
        public void Adds (AutoRetryTaskLogModel[] adds)
        {
            this._LogDAL.Adds(adds);
        }
    }
}
