using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerType
{
    /// <summary>
    /// 获取服务组下的类别
    /// </summary>
    [IRemoteConfig("sys.store.service")]
    public class GetServerTypeByGroup : RpcRemoteArray<Model.ServerType>
    {
        public long GroupId
        {
            get;
            set;
        }
    }
}
