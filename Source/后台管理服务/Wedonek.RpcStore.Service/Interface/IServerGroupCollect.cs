using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IServerGroupCollect
        {
                long AddGroup(ServerGroupDatum add);
                bool CheckIsRepeat(string typeVal);
                void DropGroup(long id);
                ServerGroup GetGroup(long id);
                ServerGroup[] GetGroup(long[] ids);
                ServerGroup[] GetGroups();
                ServerGroup[] QueryGroup(string name, IBasicPage paging, out long count);
                void SetGroup(long id, string name);
        }
}