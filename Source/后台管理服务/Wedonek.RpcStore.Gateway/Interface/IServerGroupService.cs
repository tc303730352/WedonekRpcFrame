using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IServerGroupService
        {
                long AddGroup(ServerGroupDatum add);
                bool CheckIsRepeat(string typeVal);
                void DropGroup(long id);
                ServerGroup GetGroup(long id);
                ServerGroup[] GetGroups();
                ServerGroup[] Query(string name, IBasicPage paging, out long count);
                void SetGroup(long id, string name);
        }
}