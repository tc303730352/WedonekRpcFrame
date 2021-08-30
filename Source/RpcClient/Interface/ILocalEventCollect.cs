using System.Reflection;
using System.Threading.Tasks;

namespace RpcClient.Interface
{
        public interface ILocalEventCollect
        {
                void Public(object eventData, string name = null);
                void AsyncPublic(object eventData, string name = null);
                bool RegEvent<T>(IEventHandler<T> handler);
                void RegLocalEvent(Assembly assembly);
                void RemoveEvent<T>(string name = null);
        }
}