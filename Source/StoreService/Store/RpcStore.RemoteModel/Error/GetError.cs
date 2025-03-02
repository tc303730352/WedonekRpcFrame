using WeDonekRpc.Client;
using RpcStore.RemoteModel.Error.Model;

namespace RpcStore.RemoteModel.Error
{
    /// <summary>
    /// 获取错误信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetError : RpcRemote<ErrorDatum>
    {
        /// <summary>
        /// 错误ID
        /// </summary>
        public string ErrorCode
        {
            get;
            set;
        }
    }
}
