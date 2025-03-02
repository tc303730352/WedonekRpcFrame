using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerRegion.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Helper.Area;

namespace RpcStore.Service.lmpl
{
    internal class RegionService : IRegionService
    {
        private readonly IServerRegionCollect _Region;
        private readonly IServerCollect _Server;
        public RegionService (IServerRegionCollect service, IServerCollect server)
        {
            this._Server = server;
            this._Region = service;
        }

        public int Add (RegionDatum datum)
        {
            return this._Region.Add(datum);
        }
        public RegionBasic[] GetSelectItems ()
        {
            return this._Region.GetBasics();
        }
        public void Drop (int id)
        {
            if (this._Server.CheckRegion(id))
            {
                throw new Exception("rpc.store.region.already.use");
            }
            ServerRegionModel region = this._Region.Get(id);
            this._Region.Delete(region);
        }
        public string GetName (int id)
        {
            return this._Region.GetName(id);
        }
        public RegionDto Get (int id)
        {
            ServerRegionModel region = this._Region.Get(id);
            return region.ConvertMap<ServerRegionModel, RegionDto>();
        }
        public void Update (int id, RegionDatum set)
        {
            ServerRegionModel region = this._Region.Get(id);
            _ = this._Region.Set(region, set);
        }
        public void CheckRegionName (string name)
        {
            this._Region.CheckRegionName(name);
        }

        public PagingResult<RegionData> Query (IBasicPage basicPage)
        {
            RegionDto[] list = this._Region.Query(basicPage, out int count);
            if (list.IsNull())
            {
                return new PagingResult<RegionData>(count);
            }
            Dictionary<int, int> serverNum = this._Server.GetReginServerNum(list.ConvertAll(c => c.Id));
            RegionData[] datas = list.ConvertAll<RegionDto, RegionData>(a => new RegionData
            {
                Id = a.Id,
                Address = a.Address,
                RegionName = a.RegionName,
                Phone = a.Phone,
                ProCity = AreaHelper.GetFullAreaName(a.DistrictId.HasValue ? a.DistrictId.Value : a.CityId),
                ServerNum = serverNum.GetValueOrDefault(a.Id),
                Contacts = a.Contacts
            });
            return new PagingResult<RegionData>(datas, count);
        }

        public Dictionary<int, string> GetName (int[] ids)
        {
            return this._Region.GetNames(ids);
        }
    }
}
