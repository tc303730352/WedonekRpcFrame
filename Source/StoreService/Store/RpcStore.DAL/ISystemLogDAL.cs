using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysError;
using RpcStore.RemoteModel.SysLog.Model;

namespace RpcStore.DAL
{
    public interface ISystemLogDAL
    {
        SystemErrorLogModel Get(long id);
        SysErrorLog[] Query(SysLogQuery query, IBasicPage paging, out int count);
    }
}