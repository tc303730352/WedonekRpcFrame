using RpcModel;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class ServerGroupService : IServerGroupService
        {
                private readonly IServerGroupCollect _Group = null;

                public ServerGroupService(IServerGroupCollect group)
                {
                        this._Group = group;
                }
                public long AddGroup(ServerGroupDatum add)
                {
                        return this._Group.AddGroup(add);
                }

                public bool CheckIsRepeat(string typeVal)
                {
                        return this._Group.CheckIsRepeat(typeVal);
                }

                public void DropGroup(long id)
                {
                        this._Group.DropGroup(id);
                }

                public ServerGroup GetGroup(long id)
                {
                        return this._Group.GetGroup(id);
                }

                public ServerGroup[] GetGroup(long[] ids)
                {
                        return this._Group.GetGroup(ids);
                }

                public ServerGroup[] GetGroups()
                {
                        return this._Group.GetGroups();
                }

                public ServerGroup[] Query(string name, IBasicPage paging, out long count)
                {
                        return this._Group.QueryGroup(name, paging, out count);
                }

                public void SetGroup(long id, string name)
                {
                        this._Group.SetGroup(id, name);
                }
        }
}
