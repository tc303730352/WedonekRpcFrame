using HttpService.Interface;

namespace HttpApiGateway.Interface
{
        public interface IApiHandler : IHttpHandler
        {
                bool IsEnd { get; }
        }
}
