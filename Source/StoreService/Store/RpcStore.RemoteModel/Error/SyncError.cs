using WeDonekRpc.Client;
using RpcStore.RemoteModel.Error.Model;

namespace RpcStore.RemoteModel.Error
{
    /// <summary>
    /// 同步错误信息（有修改，无添加）
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SyncError : RpcRemote<long>
    {
        public ErrorDatum Datum { get; set; }
    }
}
