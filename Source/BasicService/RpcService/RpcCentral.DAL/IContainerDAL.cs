using RpcCentral.Model;
using RpcCentral.Model.DB;

namespace RpcCentral.DAL
{
    public interface IContainerDAL
    {
        ContainerDatum Get (long id);
        void SetInternal (long id, string ip, int port);
        ContrainerBasic Find (long groupId, string conId);
        void Add (Contrainer contrainer);
    }
}