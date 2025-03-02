using RpcStore.DAL;
using RpcStore.Model.ContainerGroup;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ContainerGroup.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ContainerGroupCollect : IContainerGroupCollect
    {
        private readonly IContainerGroupDAL _ContainerGroup;

        public ContainerGroupCollect (IContainerGroupDAL containerGroup)
        {
            this._ContainerGroup = containerGroup;
        }
        public void Delete (ContainerGroupModel source)
        {
            this._ContainerGroup.Delete(source.Id);
        }
        public ContainerGroupModel[] Query (ContainerGroupQuery query, IBasicPage paging, out int count)
        {
            return this._ContainerGroup.Query(query, paging, out count);
        }

        public long Add (ContainerGroupAdd data)
        {
            if (this._ContainerGroup.CheckHostMac(data.HostMac, data.ContainerType))
            {
                throw new ErrorException("rpc.store.container.host.mac.repeat");
            }
            else if (this._ContainerGroup.CheckName(data.Name))
            {
                throw new ErrorException("rpc.store.container.group.name.repeat");
            }
            return this._ContainerGroup.Add(data);
        }
        public ContainerGroupModel Get (long id)
        {
            return this._ContainerGroup.Get(id);
        }

        public ContainerGroupDto[] Gets (int? regionId)
        {
            return this._ContainerGroup.Gets(regionId);
        }

        public Dictionary<long, string> GetNames (long[] ids)
        {
            if (ids.IsNull())
            {
                return null;
            }
            return this._ContainerGroup.GetNames(ids);
        }

        public ContainerGroupDto[] Gets (long[] ids)
        {
            if (ids.IsNull())
            {
                return null;
            }
            return this._ContainerGroup.Gets(ids);
        }

        public void Set (ContainerGroupModel source, ContainerGroupSet set)
        {
            if (source.Name == set.Name && source.Remark == set.Remark)
            {
                return;
            }
            else if (source.Name != set.Name && this._ContainerGroup.CheckName(set.Name))
            {
                throw new ErrorException("rpc.store.container.group.name.repeat");
            }
            this._ContainerGroup.Set(source.Id, set);
        }

        public string GetName (long id)
        {
            return this._ContainerGroup.GetName(id);
        }
    }
}
