using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysError;
using RpcStore.RemoteModel.SysLog.Model;

namespace RpcStore.Collect.lmpl
{
    internal class SystemLogCollect : ISystemLogCollect
    {
        private readonly ISystemLogDAL _BasicDAL;

        public SystemLogCollect (ISystemLogDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public SystemErrorLogModel Get (long id)
        {
            SystemErrorLogModel log = this._BasicDAL.Get(id);
            if (log == null)
            {
                throw new ErrorException("rpc.store.log.not.find");
            }
            return log;
        }
        public SysErrorLog[] Query (SysLogQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
    }
}
