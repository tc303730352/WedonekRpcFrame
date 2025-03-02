using WeDonekRpc.Client;
using WeDonekRpc.Helper.IdGenerator;
using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class SystemEventLogDAL : ISystemEventLogDAL
    {
        private readonly IRpcExtendResource<SystemEventLog> _BasicDAL;
        public SystemEventLogDAL (IRpcExtendResource<SystemEventLog> dal)
        {
            this._BasicDAL = dal;
        }
        public void Adds (SysEventAddLog[] logs)
        {
            this._BasicDAL.Insert(logs.ConvertMap<SysEventAddLog, SystemEventLog>((a, b) =>
            {
                b.Id = IdentityHelper.CreateId();
                return b;
            }));
        }
    }
}
