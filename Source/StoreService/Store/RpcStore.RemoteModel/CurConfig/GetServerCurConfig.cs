using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.CurConfig
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerCurConfig : RpcRemote<Model.CurConfigModel>
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId { get; set; }
    }
}
