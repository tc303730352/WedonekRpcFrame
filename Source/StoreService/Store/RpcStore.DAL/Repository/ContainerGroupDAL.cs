using RpcStore.Model.ContainerGroup;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ContainerGroup.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ContainerGroupDAL : IContainerGroupDAL
    {
        private readonly IRepository<ContainerGroupModel> _BasicDAL;
        public ContainerGroupDAL (IRepository<ContainerGroupModel> dal)
        {
            this._BasicDAL = dal;
        }
        public void Set (long id, ContainerGroupSet set)
        {
            if (!this._BasicDAL.Update(set, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.container.group.set.fail");
            }
        }
        public long Add (ContainerGroupAdd data)
        {
            ContainerGroupModel add = data.ConvertMap<ContainerGroupAdd, ContainerGroupModel>();
            add.CreateTime = DateTime.Now;
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public bool CheckHostMac (string mac, ContainerType type)
        {
            return this._BasicDAL.IsExist(a => a.HostMac == mac && a.ContainerType == type);
        }
        public bool CheckName (string name)
        {
            return this._BasicDAL.IsExist(a => a.Name == name);
        }
        public ContainerGroupModel[] Query (ContainerGroupQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }
        public ContainerGroupDto[] Gets (int? regionId)
        {
            if (regionId.HasValue)
            {
                return this._BasicDAL.Gets<ContainerGroupDto>(a => a.RegionId == regionId.Value);
            }
            return this._BasicDAL.GetAll<ContainerGroupDto>();
        }

        public ContainerGroupDto[] Gets (long[] ids)
        {
            return this._BasicDAL.Gets<ContainerGroupDto>(a => ids.Contains(a.Id));
        }
        public ContainerGroupModel Get (long id)
        {
            ContainerGroupModel model = this._BasicDAL.GetById(id);
            if (model == null)
            {
                throw new ErrorException("rpc.store.container.group.not.find");
            }
            return model;
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.container.group.delete.fail");
            }
        }

        public Dictionary<long, string> GetNames (long[] ids)
        {
            return this._BasicDAL.Gets(a => ids.Contains(a.Id), a => new
            {
                a.Id,
                a.Name
            }).ToDictionary(a => a.Id, a => a.Name);
        }

        public string GetName (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id, c => c.Name);
        }
    }
}
