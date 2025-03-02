using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace RpcStore.Service.Interface
{
    public interface IRegionService
    {
        int Add (RegionDatum datum);
        void CheckRegionName (string name);
        void Drop (int id);
        RegionDto Get (int id);
        string GetName (int id);
        Dictionary<int, string> GetName (int[] ids);
        RegionBasic[] GetSelectItems ();
        PagingResult<RegionData> Query (IBasicPage basicPage);
        void Update (int id, RegionDatum set);
    }
}