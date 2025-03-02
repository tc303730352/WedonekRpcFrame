using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.VisitCensus
{
    /// <summary>
    /// 设置服务节点的访问统计备注信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetVisitCensusShow : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Show
        {
            get;
            set;
        }
    }
}
