using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.RemoteModel.SysConfig
{
    /// <summary>
    /// 查询系统配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QuerySysConfig : BasicPage<ConfigQueryData>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public QuerySysParam Query
        {
            get;
            set;
        }
    }
}
