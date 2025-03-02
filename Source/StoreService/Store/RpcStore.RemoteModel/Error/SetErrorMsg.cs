using WeDonekRpc.Client;
using RpcStore.RemoteModel.Error.Model;

namespace RpcStore.RemoteModel.Error
{
    /// <summary>
    /// 设置错误信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetErrorMsg : RpcRemote
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public ErrorSet Datum
        {
            get;
            set;
        }
    }
}
