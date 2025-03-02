using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace RpcStore.DAL
{
    public interface IServerRegionDAL
    {
        Dictionary<int, string> GetNames (int[] ids);
        int Add (RegionDatum name);
        bool CheckName (string name);
        ServerRegionModel Get (int id);
        void Drop (int id);
        RegionBasic[] GetBasics ();
        RegionDto[] Gets (int[] ids);
        bool Set (ServerRegionModel region, RegionDatum datum);
        RegionDto[] Query (IBasicPage basicPage, out int count);
        string GetName (int id);
        void Delete(int id);
    }
}