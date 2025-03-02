using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.DictateNode
{
    /// <summary>
    /// 设置广播指令节点路由名称
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetDictateName : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
