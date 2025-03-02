using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ResourceShield;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace RpcStore.Collect
{
    public interface IResourceShieldCollect
    {
        ResourceShieldKeyState[] CheckIsShieId (long resourceId, string[] shieKey);
        ResourceShieldModel Find (string shieKey, string resourcePath);
        ResourceShieldState[] CheckIsShieId (long[] resourceId);
        void Delete (ResourceShieldModel shield);
        void Delete (ResourceShieldModel[] shield);
        ResourceShieldModel Get (long id);
        ResourceShieldModel[] GetByResourceId (long resourceId);
        ResourceShieldModel[] Query (ResourceShieldQuery query, IBasicPage paging, out int count);
        void Sync (ResourceShieldModel[] shieIds);
    }
}