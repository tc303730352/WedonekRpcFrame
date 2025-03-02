using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysLog.Model;

namespace RpcStore.RemoteModel.SysLog
{
    /// <summary>
    /// 查询日志
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QuerySysLog : BasicPage<SystemLog>
    {
        public SysLogQuery Query
        {
            get;
            set;
        }
    }
}
