using RpcCentral.Model;

namespace RpcCentral.Collect
{
    public interface IContainerGroupCollect
    {
        BasicContainerGroup GroupLogin (string mac, string localIp);
    }
}