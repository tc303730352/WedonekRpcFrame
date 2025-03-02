namespace WeDonekRpc.SqlSugar
{
    public interface ITransactionService
    {
        ILocalTransaction ApplyTran(bool isInherit=true);
    }
}