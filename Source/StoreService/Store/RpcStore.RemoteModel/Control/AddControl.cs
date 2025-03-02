using WeDonekRpc.Client;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.RemoteModel.Control
{
    /// <summary>
    /// 添加服务中心
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddControl : RpcRemote<int>
    {
        /// <summary>
        /// 中控服务资料
        /// </summary>
        public RpcControlDatum Datum
        {
            get;
            set;
        }
    }
}
