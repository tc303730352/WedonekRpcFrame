using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerGroup
{
    /// <summary>
    /// 检查服务组类型值是否重复
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CheckGroupTypeVal : RpcRemote
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
