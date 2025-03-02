using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerType
{
    /// <summary>
    /// 获取服务类别资料
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerType : RpcRemote<Model.ServerType>
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
