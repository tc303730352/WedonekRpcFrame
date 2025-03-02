using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Interface
{
    public interface IGetServiceListService
    {
        GetServerListRes GetServerList(GetServerList obj, Source source);
    }
}