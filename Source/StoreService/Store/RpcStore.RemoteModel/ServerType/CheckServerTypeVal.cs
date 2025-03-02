using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerType
{
    /// <summary>
    /// 检查类别值是否重复
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CheckServerTypeVal : RpcRemote
    {
        /// <summary>
        /// 类别值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
    }
}
