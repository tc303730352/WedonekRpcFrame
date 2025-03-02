using RpcCentral.Model;

namespace RpcCentral.Collect
{
    public interface ISystemTypeCollect
    {
        SystemTypeDatum GetSystemType(string type);

        long GetSystemTypeId(string type);
        void Refresh(string type);
    }
}