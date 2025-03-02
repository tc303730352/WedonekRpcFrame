using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ServerRegion;
using RpcStore.RemoteModel.ServerRegion.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ServerRegionEvent : IRpcApiService
    {
        private readonly IRegionService _Service;

        public ServerRegionEvent (IRegionService service)
        {
            this._Service = service;
        }

        public int AddRegion (AddRegion add)
        {
            return this._Service.Add(add.Datum);
        }
        public string GetRegionName (GetRegionName obj)
        {
            return this._Service.GetName(obj.Id);
        }
        public Dictionary<int, string> GetRegionNames (GetRegionNames obj)
        {
            return this._Service.GetName(obj.Ids);
        }
        public void CheckRegionName (CheckRegionName obj)
        {
            this._Service.CheckRegionName(obj.RegionName);
        }

        public void DropRegion (DropRegion obj)
        {
            this._Service.Drop(obj.Id);
        }

        public RegionDto GetRegion (GetRegion obj)
        {
            return this._Service.Get(obj.Id);
        }

        public RegionBasic[] GetRegionList ()
        {
            return this._Service.GetSelectItems();
        }
        public PagingResult<RegionData> QueryRegion (QueryRegion query)
        {
            return this._Service.Query(query.ToBasicPage());
        }

        public void SetRegion (SetRegion set)
        {
            this._Service.Update(set.Id, set.Datum);
        }
    }
}
