using RpcCentral.Model;

namespace RpcCentral.DAL
{
    public interface IContainerGroupDAL
    {
        BasicContainerGroup Find (string number);
        void SetLocalIp (int id, string localIp);
    }
}