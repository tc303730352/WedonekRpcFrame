using WeDonekRpc.Helper;
using RpcSync.Collect;
using RpcSync.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Service
{
    internal class NodeLoadService : INodeLoadService
    {
        private IServerGroupCollect _ServerGroup;
        private ISystemTypeCollect _ServerType;
        private IDictateNodeCollect _DictateNode;
        private INodeService _NodeServer;

        public NodeLoadService(IServerGroupCollect serverGroup,
            INodeService nodeServer,
            IDictateNodeCollect dictateNode,
            ISystemTypeCollect serverType)
        {
            this._NodeServer = nodeServer;
            this._DictateNode = dictateNode;
            this._ServerGroup = serverGroup;
            this._ServerType = serverType;
        }
        public void LoadServerNode()
        {
            SystemType[] sysType = this._ServerType.GetSystemTypes();
            if (sysType.IsNull())
            {
                return;
            }
            ServerGroup[] groups = this._ServerGroup.GetGroups();
            this._NodeServer.LoadServerNode(groups, sysType);
        }

        public void LoadDictateNode()
        {
            DictateNode[] nodes = _DictateNode.GetDictateNode();
            if (nodes.Length == 0)
            {
                return;
            }
            this._NodeServer.LoadDictateNode(nodes);
        }
    }
}
