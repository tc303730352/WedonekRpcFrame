using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ResourceShield;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace RpcStore.DAL
{
    public interface IResourceShieldDAL
    {
        ResourceShieldState[] CheckIsShieId (long[] resourceId);
        ResourceShieldModel Find (string shieKey, string resourcePath);
        void Delete (long id);
        void Delete (long[] id);
        ResourceShieldModel Get (long id);
        ResourceShieldModel[] GetByResourceId (long resourceId);
        ResourceShieldModel[] Query (ResourceShieldQuery query, IBasicPage paging, out int count);
        ResourceShieldKeyState[] CheckIsShieId (long resourceId, string[] shieKey);
        void Sync (ResourceShieldModel[] shieIds);
    }
}