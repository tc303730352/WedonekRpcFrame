using RpcStore.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ContainerDAL : IContainerDAL
    {
        private readonly IRepository<ContainerListModel> _BasicDAL;
        public ContainerDAL (IRepository<ContainerListModel> dal)
        {
            this._BasicDAL = dal;
        }
        public Dictionary<long, string> GetInternalAddr (long[] ids)
        {
            return this._BasicDAL.Gets(a => ids.Contains(a.Id), a => new
            {
                a.Id,
                a.InternalIp,
                a.InternalPort
            }).ToDictionary(a => a.Id, a => a.InternalIp + ":" + a.InternalPort);
        }
        public void Clear (long groupId)
        {
            if (!this._BasicDAL.Delete(a => a.GroupId == groupId))
            {
                throw new ErrorException("rpc.store.container.clear.fail");
            }
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.container.delete.fail");
            }
        }

        public string GetInternalAddr (long id)
        {
            var data = this._BasicDAL.Get(a => a.Id == id, a => new
            {
                a.InternalIp,
                a.InternalPort
            });
            return data.InternalIp + ":" + data.InternalPort;
        }
    }
}
