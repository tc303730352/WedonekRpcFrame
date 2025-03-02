namespace WeDonekRpc.SqlSugar.Repository
{
    internal class DefRepository<T> : BasicRepository<T> where T : class, new()
    {
        public DefRepository (ISqlClientFactory factory) : base(factory, "default")
        {

        }
    }
}
