using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerRegion;
using RpcStore.RemoteModel.ServerRegion.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class RegionService : IRegionService
    {

        public int Add (RegionDatum add)
        {
            return new AddRegion
            {
                Datum = add
            }.Send();
        }
        public void Delete(int id)
        {
            new DropRegion
            {
                Id = id
            }.Send();
        }
        public void Update (int id, RegionDatum datum)
        {
            new SetRegion
            {
                Datum = datum,
                Id = id
            }.Send();
        }
        public PagingResult<RegionData> Query (IBasicPage paging)
        {
            return new QueryRegion
            {
                NextId = paging.NextId,
                Size = paging.Size,
                SortName = paging.SortName,
                Index = paging.Index,
                IsDesc = paging.IsDesc
            }.Send();
        }
        public RegionBasic[] GetRegionList ()
        {
            return new GetRegionList().Send();
        }
        public RegionDto Get (int id)
        {
            return new GetRegion
            {
                Id = id
            }.Send();
        }
        public void CheckName (string name)
        {
            new CheckRegionName
            {
                RegionName = name
            }.Send();
        }
    }
}
