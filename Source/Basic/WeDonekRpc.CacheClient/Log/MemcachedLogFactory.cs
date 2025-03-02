using Microsoft.Extensions.Logging;

namespace WeDonekRpc.CacheClient.Log
{
    internal class MemcachedLogFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {

        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MemcachedLog(categoryName);
        }

        public void Dispose()
        {

        }
    }
}
