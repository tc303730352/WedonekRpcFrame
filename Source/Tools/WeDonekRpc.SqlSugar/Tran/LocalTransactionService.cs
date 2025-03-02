namespace WeDonekRpc.SqlSugar.Tran
{
    internal class LocalTransactionService : ITransactionService
    {
        private ISqlClientFactory _Factory;
        public LocalTransactionService(ISqlClientFactory factory)
        {
            this._Factory = factory;
        }

        public ILocalTransaction ApplyTran(bool isInherit = true)
        {
            return new LocalTransaction(_Factory, isInherit);
        }
    }
}
