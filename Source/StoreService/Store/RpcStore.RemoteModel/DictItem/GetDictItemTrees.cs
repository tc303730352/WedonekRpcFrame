using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictItem.Model;

namespace RpcStore.RemoteModel.DictItem
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetDictItemTrees : RpcRemoteArray<DictTreeItem>
    {
        public string IndexCode { get; set; }
    }
}
