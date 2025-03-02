using RpcSync.Model;
using RpcSync.Service.Node;

namespace RpcSync.Service.Interface
{
    public interface INodeService
    {
        RootNode[] GetRootNode();
        void Load(string key, List<RootNode> dictates);

        void Load(string key, List<long> dictates);
        void LoadDictateNode(DictateNode[] nodes);
        void LoadServerNode(ServerGroup[] groups, SystemType[] sysTypes);
    }
}