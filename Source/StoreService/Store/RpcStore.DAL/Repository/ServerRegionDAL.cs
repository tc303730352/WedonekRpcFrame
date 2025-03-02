using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerRegion.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    public class ServerRegionDAL : IServerRegionDAL
    {
        private readonly IRepository<ServerRegionModel> _BasicDAL;
        public ServerRegionDAL(IRepository<ServerRegionModel> dal)
        {
            this._BasicDAL = dal;
        }
        public ServerRegionModel Get(int id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public string GetName(int id)
        {
            return this._BasicDAL.Get(c => c.Id == id, c => c.RegionName);
        }
        public Dictionary<int, string> GetNames(int[] ids)
        {
            return this._BasicDAL.Gets(c => ids.Contains(c.Id)).ToDictionary(c => c.Id, c => c.RegionName);
        }
        public bool CheckName(string name)
        {
            return this._BasicDAL.IsExist(c => c.RegionName == name && c.IsDrop == false);
        }

        public RegionBasic[] GetBasics()
        {
            return this._BasicDAL.Gets<RegionBasic>(c => c.IsDrop == false);
        }
        public RegionDto[] Gets(int[] ids)
        {
            return this._BasicDAL.Gets<RegionDto>(c => ids.Contains(c.Id));
        }
        public int Add(RegionDatum datum)
        {
            ServerRegionModel add = datum.ConvertMap<RegionDatum, ServerRegionModel>();
            return (int)this._BasicDAL.InsertReutrnIdentity(add);
        }
        public bool Set(ServerRegionModel region, RegionDatum datum)
        {
            return this._BasicDAL.Update<RegionDatum>(region, datum);
        }
        public void Drop(int id)
        {
            if (!this._BasicDAL.Update(c => c.IsDrop == true, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.region.drop.error");
            }
        }
        public void Delete(int id)
        {
            if (!this._BasicDAL.Delete(c => c.Id == id))
            {
                throw new ErrorException("rpc.store.region.delete.error");
            }
        }
        public RegionDto[] Query(IBasicPage basicPage, out int count)
        {
            return this._BasicDAL.Query<RegionDto>(a => a.IsDrop == false, basicPage, out count);
        }
    }
}
