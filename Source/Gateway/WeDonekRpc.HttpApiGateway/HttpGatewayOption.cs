using System;
using System.Reflection;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Route;
using WeDonekRpc.HttpService.Config;

namespace WeDonekRpc.HttpApiGateway
{
    internal class HttpGatewayOption : IHttpGatewayOption
    {
        private readonly IGatewayOption _Option;
        private readonly IApiModular _ApiModular;
        private Type _DefApiServiceEvent = typeof(ApiServiceEvent);
        internal HttpGatewayOption (IGatewayOption option, IApiModular apiModular)
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
            _ = this.IocBuffer.Register(typeof(IApiServiceEvent), this._DefApiServiceEvent, this._ApiModular.ServiceName);
        }

        public void RegModular (IModular modular)
        {
            this._Option.RegModular(modular);
        }
        public void AddFileDir (FileDirConfig dir)
        {
            HttpService.HttpService.AddFileDir(dir);
        }
        public void SetDefApiServiceEvent<T> () where T : class, IApiServiceEvent
        {
            this._DefApiServiceEvent = typeof(T);
        }
        internal void Init ()
        {
            this.Route.Init();
        }
    }
}
