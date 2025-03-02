using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.MerConfig
{
    /// <summary>
    /// 添加集群系统类别配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetMerConfig : RpcRemote<long>
    {
        /// <summary>
        /// 集群系统类别配置
        /// </summary>
        public Model.MerConfigArg Config
        {
            get;
            set;
        }
    }
}
