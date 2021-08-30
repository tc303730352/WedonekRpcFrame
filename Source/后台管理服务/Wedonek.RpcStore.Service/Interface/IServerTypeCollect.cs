using RpcModel;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IServerTypeCollect
        {
                long AddServiceType(ServerTypeDatum add);
                bool CheckIsRepeat(string typeVal);
                void DropServiceType(long id);
                ServerType GetServiceType(long id);
                ServerType[] GetServiceTypes(long[] ids);
                ServerType[] GetServiceTypes(long groupId);
                ServerType[] QuerySystemType(ServerTypeQueryParam query, IBasicPage paging, out long count);
                void SetServiceType(long id, ServerTypeSetParam param);
                void Clear(long groupId);
        }
}