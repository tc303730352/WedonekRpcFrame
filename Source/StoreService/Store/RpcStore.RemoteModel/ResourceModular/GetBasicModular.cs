using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ResourceModular
{
    /// <summary>
    /// 获取资源模块
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetBasicModular : RpcRemoteArray<Model.BasicModular>
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点系统类型
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
    }
}
