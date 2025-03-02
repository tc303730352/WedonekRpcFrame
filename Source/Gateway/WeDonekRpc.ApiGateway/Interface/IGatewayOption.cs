using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IGatewayOption
    {
        RpcInitOption Option { get; }
        IocBuffer IocBuffer { get; }
        void RegDoc (IApiDocModular doc);
        void RegModular (IModular modular);
    }
}