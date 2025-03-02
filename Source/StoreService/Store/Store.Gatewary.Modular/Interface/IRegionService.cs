using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IRegionService
    {
        int Add (RegionDatum add);
        void CheckName (string name);
        void Delete(int id);
        RegionDto Get (int id);
        RegionBasic[] GetRegionList ();
        PagingResult<RegionData> Query (IBasicPage paging);
        void Update (int id, RegionDatum datum);
    }
}