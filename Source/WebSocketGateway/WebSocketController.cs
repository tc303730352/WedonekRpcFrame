
using ApiGateway.Interface;

using HttpWebSocket.Model;

using WebSocketGateway.Interface;

namespace WebSocketGateway
{
        public class WebSocketController : Interface.IApiController
        {
                private readonly IApiSocketService _Service = ApiHandler.ApiService;

                public string ServiceName => _Service.ServiceName;

                public dynamic UserState => _Service.UserState;

                public IClientIdentity Identity => _Service.Identity;

                public RequestBody Head => _Service.Head;

                public virtual void Install(IRouteConfig config)
                {

                }
        }
}
