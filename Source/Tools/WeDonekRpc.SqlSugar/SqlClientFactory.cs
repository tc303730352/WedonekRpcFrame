using SqlSugar;
using WeDonekRpc.SqlSugar.Tran;

namespace WeDonekRpc.SqlSugar
{
    internal class SqlClientFactory : ISqlClientFactory
    {
        private readonly IDBConfig _Config;
        private SqlSugarClient _DbClient;

        private static readonly AsyncLocal<ILocalTran> _Current = new AsyncLocal<ILocalTran>();

        public SqlClientFactory (IDBConfig config)
        {
            this._Config = config;
        }
        protected SqlSugarClient DbClient
        {
            get
            {
                if (this._DbClient == null)
                {
                    this._DbClient = new SqlSugarClient(this._Config.Configs);
                }
                return this._DbClient;
            }
        }
        public bool IsTran => _Current.Value != null;

        public ILocalTran Current => _Current.Value;

        public void BeginTran ()
        {
            LocalTran tran = new LocalTran(this.DbClient);
            tran.BeginTran();
            _Current.Value = tran;
        }
        public void CommitTran (ILocalTran client)
        {
            if (_Current.Value != null)
            {
                _Current.Value.CommitTran();
                _Current.Value = client;
            }
        }
        public void RollbackTran (ILocalTran client)
        {
            if (_Current.Value != null)
            {
                _Current.Value.RollbackTran();
                _Current.Value = client;
            }
        }
        public SqlSugarProvider GetProvider (string name)
        {
            if (_Current.Value != null)
            {
                return _Current.Value.GetProvider(name);
            }
            return this.DbClient.GetConnection(name);
        }



        public ISugarQueryable<T> GetQueryable<T> (string name)
        {
            return this.DbClient.GetConnection(name).Queryable<T>();
        }

        public static void PendingTran (ILocalTran tran)
        {
            if (_Current.Value != null && _Current.Value.TranId == tran.TranId)
            {
                _Current.Value = null;
            }
        }

        public void Dispose ()
        {
            if (_Current.Value != null)
            {
                _Current.Value.Dispose();
                _Current.Value = null;
            }
        }
    }
}
