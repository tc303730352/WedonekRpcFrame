using WeDonekRpc.Model.Model;

namespace RpcStore.RemoteModel.ClientLimit.Model
{
    /// <summary>
    /// 服务节点限流配置
    /// </summary>
    public class ClientLimitData : ServerClientLimit
    {
        public long Id
        {
            get;
            set;
        }
    }
}
