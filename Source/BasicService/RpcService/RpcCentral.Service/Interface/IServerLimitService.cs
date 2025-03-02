using WeDonekRpc.Model.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Interface
{
    public interface IServerLimitService
    {
        LimitConfigRes GetServerLimit(GetServerLimit obj);
    }
}