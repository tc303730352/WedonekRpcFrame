using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerGroup
{
    /// <summary>
    /// 修改服务组名字
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetServerGroup : RpcRemote
    {
        /// <summary>
        /// 服务组ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
