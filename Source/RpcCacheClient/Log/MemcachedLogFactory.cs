using Microsoft.Extensions.Logging;

namespace RpcCacheClient.Log
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
