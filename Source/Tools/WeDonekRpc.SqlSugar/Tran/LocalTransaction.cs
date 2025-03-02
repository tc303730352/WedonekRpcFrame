namespace WeDonekRpc.SqlSugar.Tran
{
    internal class LocalTransaction : ILocalTransaction
    {
        private ISqlClientFactory _Factory;
        private ILocalTran _Client;
        private bool _IsInherit;
        public LocalTransaction(ISqlClientFactory factory, bool isInherit)
        {
            this._Factory = factory;
            if (factory.IsTran && isInherit)
            {
                _IsInherit = true;
                return;
            }
            else if (factory.IsTran)
            {
                _Client = factory.Current;
            }
            this._Factory.BeginTran();
        }


        public void Commit()
        {
            if (_IsInherit)
            {
                return;
            }
            this._Factory.CommitTran(_Client);
        }
        public void Dispose()
        {
            if (_IsInherit)
            {
                return;
            }
            this._Factory.RollbackTran(_Client);
        }
    }
}
