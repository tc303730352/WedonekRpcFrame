using System;
using System.Reflection;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Route;

namespace WeDonekRpc.WebSocketGateway
{
    internal class WebSocketGatewayOption : IWebSocketGatewayOption
    {
        private readonly IGatewayOption _Option;
        private readonly IApiModular _ApiModular;
        internal WebSocketGatewayOption (IGatewayOption option, IApiModular apiModular)
        {
            this._Option = option;
            this.IocBuffer = option.IocBuffer;
            this.Route = new RouteBuffer(option.IocBuffer, apiModular);
            this._ApiModular = apiModular;
        }

        public IocBuffer IocBuffer { get; }

        public RouteBuffer Route { get; }

        public IGatewayOption Option => this._Option;

        internal void Load ()
        {
            Type type = this._ApiModular.GetType();
            Assembly assembly = type.Assembly;
            this.Route.Load(assembly);
        }

        public void RegModular (IModular modular)
        {
            this._Option.RegModular(modular);
        }

        internal void Init ()
        {
            this.Route.Init();
        }
    }
}
