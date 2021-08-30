using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IServerRegionService
        {
                void SetRegion(int id, string name);
                int AddRegion(string name);
                void DropRegion(int id);
                ServerRegion[] GetServerRegion();
        }
}