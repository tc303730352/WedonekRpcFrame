using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysLog.Model;

namespace RpcStore.RemoteModel.SysLog
{
    /// <summary>
    /// 获取系统日志详细
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetSysLog : RpcRemote<SystemLogData>
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
    }
}
