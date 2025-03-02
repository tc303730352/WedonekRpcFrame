using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ResourceShield;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ResourceShieldCollect : IResourceShieldCollect
    {
        private readonly IResourceShieldDAL _BasicDAL;
        public ResourceShieldCollect (IResourceShieldDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public ResourceShieldModel Find (string shieKey, string resourcePath)
        {
            return this._BasicDAL.Find(shieKey, resourcePath);
        }
        public ResourceShieldModel[] GetByResourceId (long resourceId)
        {
            return this._BasicDAL.GetByResourceId(resourceId);
        }

        public ResourceShieldState[] CheckIsShieId (long[] resourceId)
        {
            return this._BasicDAL.CheckIsShieId(resourceId);
        }

        public ResourceShieldModel Get (long id)
        {
            ResourceShieldModel shield = this._BasicDAL.Get(id);
            if (shield == null)
            {
                throw new ErrorException("rpc.store.shieId.not.find");
            }
            return shield;
        }

        public void Delete (ResourceShieldModel shield)
        {
            this._BasicDAL.Delete(shield.Id);
        }

        public ResourceShieldModel[] Query (ResourceShieldQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }

        public ResourceShieldKeyState[] CheckIsShieId (long resourceId, string[] shieKey)
        {
            return this._BasicDAL.CheckIsShieId(resourceId, shieKey);
        }

        public void Sync (ResourceShieldModel[] shieIds)
        {
            this._BasicDAL.Sync(shieIds);
        }

        public void Delete (ResourceShieldModel[] shield)
        {
            this._BasicDAL.Delete(shield.ConvertAll(a => a.Id));
        }
    }
}
