namespace WeDonekRpc.SqlSugar
{
    public interface ILocalTransaction : IDisposable
    {
        void Commit();
    }
}