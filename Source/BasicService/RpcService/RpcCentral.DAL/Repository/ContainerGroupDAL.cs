using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ContainerGroupDAL : IContainerGroupDAL
    {
        private readonly IRepository<ContainerGroup> _Db;
        public ContainerGroupDAL (IRepository<ContainerGroup> repository)
        {
            this._Db = repository;
        }

        public BasicContainerGroup Find (string mac)
        {
            return this._Db.Get<BasicContainerGroup>(c => c.HostMac == mac);
        }

        public void SetLocalIp (int id, string localIp)
        {
            if (!this._Db.Update(a => a.HostIp == localIp, a => a.Id == id))
            {
                throw new ErrorException("rpc.container.group.set.fail");
            }
        }
    }
}
