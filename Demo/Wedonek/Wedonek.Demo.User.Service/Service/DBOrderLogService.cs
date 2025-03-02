using System;
using Wedonek.Demo.RemoteModel.DBOrderLog;
using Wedonek.Demo.User.Service.DB;
using Wedonek.Demo.User.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace Wedonek.Demo.User.Service.Service
{
    internal class DBOrderLogService : IDBOrderLogService
    {
        private readonly IRepository<DB.DBUserOrderLog> _Repository;

        public DBOrderLogService (IRepository<DBUserOrderLog> repository)
        {
            this._Repository = repository;
        }

        public long AddOrder (AddOrderLog order)
        {
            DBUserOrderLog add = order.ConvertMap<AddOrderLog, DBUserOrderLog>();
            add.Id = IdentityHelper.CreateId();
            add.AddTime = DateTime.Now;
            this._Repository.Insert(add);
            return add.Id;
        }
    }
}
