using RpcSync.Service.Node;

namespace RpcSync.Service.Interface
{
    public interface INodeServer
    {
        void Load(List<RootNode> dictates);

        void Load(List<long> sysTypeId);
    }
}