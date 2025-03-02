using RpcStore.Collect;
using RpcStore.Model.ContainerGroup;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class ContainerGroupService : IContainerGroupService
    {
        private readonly IContainerGroupCollect _Group;
        private readonly IServerRegionCollect _Region;
        private readonly IServerCollect _Server;
        public ContainerGroupService (IContainerGroupCollect group,
            IServerRegionCollect region,
            IServerCollect server)
        {
            this._Group = group;
            this._Region = region;
            this._Server = server;
        }
        public PagingResult<ContainerGroup> Query (ContainerGroupQuery query, IBasicPage paging)
        {
            ContainerGroupModel[] list = this._Group.Query(query, paging, out int count);
            ContainerGroup[] datas = list.ConvertMap<ContainerGroupModel, ContainerGroup>();
            Dictionary<int, string> regions = this._Region.GetNames(list.Distinct(c => c.RegionId));
            Dictionary<long, int> servers = this._Server.GetContainerServerNum(list.ConvertAll(c => c.Id), new RpcServiceState[]
            {
                RpcServiceState.正常,
                RpcServiceState.下线,
                RpcServiceState.待启用
            });
            datas.ForEach(c =>
            {
                c.RegionName = regions.GetValueOrDefault(c.RegionId);
                c.ServerNum = servers.GetValueOrDefault(c.Id);
            });
            return new PagingResult<ContainerGroup>(count, datas);
        }
        public long Add (ContainerGroupAdd add)
        {
            return this._Group.Add(add);
        }
        public void Set (long id, ContainerGroupSet set)
        {
            ContainerGroupModel source = this._Group.Get(id);
            this._Group.Set(source, set);
        }
        public void Delete (long id)
        {
            ContainerGroupModel source = this._Group.Get(id);
            if (this._Server.CheckIsExistByContainer(source.Id))
            {
                throw new ErrorException("rpc.store.container.group.already.use");
            }
            this._Group.Delete(source);
        }

        public ContainerGroupItem[] GetItems (int? regionId)
        {
            ContainerGroupDto[] dtos = this._Group.Gets(regionId);
            return dtos.ConvertAll(a => new ContainerGroupItem
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        public ContainerGroupDatum Get (long id)
        {
            ContainerGroupModel source = this._Group.Get(id);
            return source.ConvertMap<ContainerGroupModel, ContainerGroupDatum>();
        }
    }
}
