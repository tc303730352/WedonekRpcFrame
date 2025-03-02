using RpcSync.DAL;
using RpcSync.Model;

namespace RpcSync.Collect.Collect
{
    internal class ServerGroupCollect : IServerGroupCollect
    {
        private IServerGroupDAL _ServerGroup;

        public ServerGroupCollect(IServerGroupDAL serverGroup)
        {
            this._ServerGroup = serverGroup;
        }

        public ServerGroup[] GetGroups()
        {
            return _ServerGroup.GetServerGroup();
        }
    }
}
