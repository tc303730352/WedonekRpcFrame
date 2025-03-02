using RpcCentral.Model;

namespace RpcCentral.DAL
{
    public interface IRpcControlListDAL
    {
        RpcControl[] GetControlServer();
    }
}