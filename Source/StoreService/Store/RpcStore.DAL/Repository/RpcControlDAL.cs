using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Control.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    /// <summary>
    /// 服务中心
    /// </summary>
    internal class RpcControlDAL : IRpcControlDAL
    {
        private readonly IRepository<RpcControlModel> _BasicDAL;
        public RpcControlDAL (IRepository<RpcControlModel> dal)
        {
            this._BasicDAL = dal;
        }
        public int Add (RpcControlModel add)
        {
            return (int)this._BasicDAL.InsertReutrnIdentity(add);
        }

        public bool CheckIsRepeat (string ip, int port)
        {
            return this._BasicDAL.IsExist(c => c.ServerIp == ip && c.Port == port);
        }

        public RpcControlModel Get (int id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }

        public RpcControlModel[] Query (IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(paging, out count);
        }
        public void Set (int id, RpcControlDatum set)
        {
            if (!this._BasicDAL.Update(set, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.control.set.fail");
            }
        }
        public void Delete (int id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.control.delete.fail");
            }
        }
    }
}
