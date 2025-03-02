using SqlSugar;
using WeDonekRpc.Helper.IdGenerator;

namespace WeDonekRpc.SqlSugar.Tran
{
    internal class LocalTran : ILocalTran
    {
        private readonly SqlSugarClient _Client;
        private bool _IsEnd = false;
        public LocalTran (SqlSugarClient client)
        {
            this.TranId = IdentityHelper.CreateId();
            this._Client = client;
        }
        public long TranId { get; }
        public void BeginTran ()
        {
            this._Client.BeginTran();
        }
        public void CommitTran ()
        {
            if (!this._IsEnd)
            {
                this._Client.CommitTran();
                this._IsEnd = true;
            }
        }
        public void RollbackTran ()
        {
            if (!this._IsEnd)
            {
                this._Client.RollbackTran();
                this._IsEnd = true;
            }
        }
        public SqlSugarProvider GetProvider (string name)
        {
            return this._Client.GetConnection(name);
        }

        public void PendingTran ()
        {
            SqlClientFactory.PendingTran(this);
        }

        public void Dispose ()
        {
            if (this._IsEnd)
            {
                this._Client.Dispose();
            }
        }
    }
}
