using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ContainerDAL : IContainerDAL
    {
        private readonly IRepository<Contrainer> _Db;
        public ContainerDAL (IRepository<Contrainer> repository)
        {
            this._Db = repository;
        }
        public void SetInternal (long id, string ip, int port)
        {
            if (!this._Db.Update(a => new Contrainer
            {
                InternalIp = ip,
                InternalPort = port
            }, a => a.Id == id))
            {
                throw new Exception("rpc.contrainer.set.fail");
            }
        }
        public ContainerDatum Get (long id)
        {
            return this._Db.JoinGet<ContainerGroup, ContainerDatum>((a, b) => b.Id == a.GroupId && a.Id == id, (a, b) => new ContainerDatum
            {
                HostIp = b.HostIp,
                HostMac = b.HostMac,
                InternalIp = a.InternalIp,
                InternalPort = a.InternalPort
            });
        }
        public ContrainerBasic Find (long groupId, string conId)
        {
            return this._Db.Get<ContrainerBasic>(c => c.GroupId == groupId && c.ContrainerId == conId);
        }

        public void Add (Contrainer add)
        {
            add.Id = IdentityHelper.CreateId();
            add.AddTime = DateTime.Now;
            this._Db.Insert(add);
        }
    }
}
