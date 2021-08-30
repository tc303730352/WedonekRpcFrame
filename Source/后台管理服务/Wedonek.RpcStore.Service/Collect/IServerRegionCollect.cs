using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IServerRegionCollect
        {
                void SetRegion(int id, string name);
                int AddRegion(string name);
                void DropRegion(int id);
                ServerRegion[] GetServerRegion();
        }
}