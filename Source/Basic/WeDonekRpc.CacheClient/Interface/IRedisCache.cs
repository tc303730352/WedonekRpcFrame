namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisCache : IRedisController
    {
        string FormatKey (string key);
    }
}
