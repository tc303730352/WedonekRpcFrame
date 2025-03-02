using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.Model.SysConfig;
using RpcStore.RemoteModel.SysConfig.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class SysConfigDAL : ISysConfigDAL
    {
        private readonly IRepository<SysConfigModel> _BasicDAL;
        public SysConfigDAL (IRepository<SysConfigModel> dal)
        {
            this._BasicDAL = dal;
        }

        public SysConfigBasic[] Query (QuerySysParam query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<SysConfigBasic>(query.ToWhere(), paging, out count);
        }
        public BasicSysConfig FindBasicConfig (long rpcMerId, string name)
        {
            return this._BasicDAL.Get<BasicSysConfig>(c => c.RpcMerId == rpcMerId && c.Name == name && c.ConfigType == RpcConfigType.基础配置);
        }

        public bool CheckIsExists (SysConfigModel config)
        {
            return this._BasicDAL.IsExist(config.ToWhere());
        }

        public SysConfigModel Get (long Id)
        {
            return this._BasicDAL.Get(c => c.Id == Id);
        }
        public void Add (SysConfigModel add)
        {
            add.ToUpdateTime = DateTime.Now;
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
        }

        public SysConfigModel FindConfig (string systemType, string name)
        {
            return this._BasicDAL.Get(c => c.SystemType == systemType && c.Name == name);
        }

        public void Set (long id, SysConfigSetParam config)
        {
            if (!this._BasicDAL.Update(config, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.sys.config.set.error");
            }
        }
        public void SetIsEnable (long id, bool isEnable)
        {
            if (!this._BasicDAL.Update(new
            {
                IsEnable = isEnable,
                ToUpdateTime = DateTime.Now
            }, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.sys.config.set.error");
            }
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(c => c.Id == id))
            {
                throw new ErrorException("rpc.store.sys.config.delete.error");
            }
        }

        public void Clear (long serverId)
        {
            _ = this._BasicDAL.Delete(c => c.ServerId == serverId);
        }
    }
}
