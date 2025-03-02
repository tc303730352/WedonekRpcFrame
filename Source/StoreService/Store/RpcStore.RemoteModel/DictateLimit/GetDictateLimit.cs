using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.DictateLimit
{
    /// <summary>
    /// 获取指令限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetDictateLimit : RpcRemote<Model.DictateLimit>
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
