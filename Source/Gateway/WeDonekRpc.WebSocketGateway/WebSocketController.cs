using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.Modular;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway
{
    public class WebSocketController : IApiController
    {
        private readonly IApiSocketService _Service = ApiHandler.ApiService.Value;

        public string ServiceName => this._Service.ServiceName;

        public IUserState UserState => this._Service.UserState;

        public IUserIdentity Identity => this._Service.Identity;

        public RequestBody Head => this._Service.Head;

        public virtual void Install (IRouteConfig config)
        {

        }
    }
}
