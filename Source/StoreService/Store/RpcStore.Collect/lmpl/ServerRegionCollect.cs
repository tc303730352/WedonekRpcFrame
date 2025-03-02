using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerRegionCollect : IServerRegionCollect
    {
        private readonly IServerRegionDAL _BasicDAL;

        public ServerRegionCollect (IServerRegionDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public ServerRegionModel Get (int id)
        {
            ServerRegionModel region = this._BasicDAL.Get(id);
            if (region == null || region.IsDrop)
            {
                throw new ErrorException("rpc.store.region.not.find");
            }
            return region;
        }
        public RegionBasic[] GetBasics ()
        {
            return this._BasicDAL.GetBasics();
        }
        public void CheckRegionName (string name)
        {
            if (this._BasicDAL.CheckName(name))
            {
                throw new ErrorException("rpc.store.region.name.repeat");
            }
        }
        public bool Set (ServerRegionModel region, RegionDatum datum)
        {
            if (region.RegionName != datum.RegionName)
            {
                this.CheckRegionName(datum.RegionName);
            }
            return this._BasicDAL.Set(region, datum);
        }

        public int Add (RegionDatum datum)
        {
            this.CheckRegionName(datum.RegionName);
            return this._BasicDAL.Add(datum);
        }
        public string GetName (int id)
        {
            return this._BasicDAL.GetName(id);
        }
        public Dictionary<int, string> GetNames (int[] ids)
        {
            if (ids.IsNull())
            {
                return null;
            }
            return this._BasicDAL.GetNames(ids);
        }
        public void Delete (ServerRegionModel region)
        {
            this._BasicDAL.Delete(region.Id);
        }
        public void Drop(ServerRegionModel region)
        {
            this._BasicDAL.Drop(region.Id);
        }
        public RegionDto[] Gets (int[] ids)
        {
            return this._BasicDAL.Gets(ids);
        }

        public RegionDto[] Query (IBasicPage basicPage, out int count)
        {
            return this._BasicDAL.Query(basicPage, out count);
        }
    }
}
