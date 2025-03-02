using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.Interface
{
        public interface IApiHandler : IHttpHandler
        {
                bool IsEnd { get; }
        }
}
