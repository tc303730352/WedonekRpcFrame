using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetRegion : RpcRemote
    {
        public int Id
        {
            get;
            set;
        }
        public RegionDatum Datum { get; set; }
    }
}
