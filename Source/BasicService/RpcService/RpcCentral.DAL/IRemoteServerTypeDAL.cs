using RpcCentral.Model;

namespace RpcCentral.DAL
{
    public interface IRemoteServerTypeDAL
    {
        long GetSystemTypeId(string typeVal);
        SystemTypeDatum GetSystemType(string typeVal);
    }
}