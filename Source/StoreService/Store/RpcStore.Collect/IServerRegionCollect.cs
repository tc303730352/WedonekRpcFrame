using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace RpcStore.Collect
{
    public interface IServerRegionCollect
    {
        void CheckRegionName (string name);
        int Add (RegionDatum name);
        void Delete (ServerRegionModel region);
        ServerRegionModel Get (int id);
        Dictionary<int, string> GetNames (int[] ids);
        RegionBasic[] GetBasics ();
        RegionDto[] Gets (int[] ids);
        bool Set (ServerRegionModel region, RegionDatum datum);
        RegionDto[] Query (IBasicPage basicPage, out int count);
        string GetName (int regionId);
        void Drop(ServerRegionModel region);
    }
}