using System.Reflection;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.PlugIn;
using WeDonekRpc.HttpService.Interface;
namespace WeDonekRpc.HttpApiGateway.Idempotent
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class IdempotentService : IIdempotentService, IIdempotentHandler
    {
        private readonly IIdempotentConfig _Config;
        private readonly IIocService _Unity;
        private readonly IApiModular _Modula;
        private IdempotentType _Type = IdempotentType.关闭;
        private IIdempotent _Idem;
        private readonly IPlugInService _PlugIn;

        private string _RouteId;

        public IdempotentService ( IIdempotentConfig config,
            IIocService unity,
            IPlugInService plugIn,
            IApiModularService modular )
        {
            this._PlugIn = plugIn;
            this._Config = config;
            this._Unity = unity;
            this._Modula = modular.GetModular(GatewayModular.ModularName);
        }
        public void Init ()
        {
            this._Config.RefreshEvent += this._Config_RefreshEvent;
            this._Reset();
        }

        private void _Config_RefreshEvent ( IIdempotentConfig obj )
        {
            this._Reset();
        }
        public bool CheckIsLimit ( string path )
        {
            return this._Config.CheckIsLimit(path);
        }
        public string GetTokenId ( Interface.IRoute route, IHttpHandler handler )
        {
            if ( this._Type == IdempotentType.关闭 )
            {
                return null;
            }
            else if ( this._Config.RequestMehod == RequestMehod.Head )
            {
                return handler.Request.Headers[this._Config.ArgName];
            }
            else
            {
                return handler.Request.QueryString[this._Config.ArgName];
            }

        }
        private void _Reset ()
        {
            if ( this._Config.IdempotentType == this._Type )
            {
                return;
            }
            this._Dispose();
            this._Type = this._Config.IdempotentType;
            if ( this._Config.IdempotentType == IdempotentType.Token )
            {
                this._Idem = this._Unity.Resolve<ITokenIdempotent>();
            }
            else if ( this._Config.IdempotentType == IdempotentType.Key )
            {
                this._Idem = this._Unity.Resolve<IKeyIdempotent>();
            }
            this._PlugIn.Add(new IdempotentPlugIn(this._Idem, this));
            if ( this._Config.IdempotentType == IdempotentType.Token && this._Config.IsEnableRoute )
            {
                MethodInfo method = typeof(IdempotentTokenApi).GetMethod("ApplyToken");
                this._RouteId = this._Modula.Route.RegApi(method, this._Config.TokenRoute);
            }
        }
        private void _Dispose ()
        {
            if ( this._Type == IdempotentType.Token && this._Config.IsEnableRoute && !this._RouteId.IsNull() )
            {
                this._Modula.Route.Remove(this._RouteId);
            }
            this._PlugIn.Delete("Idempotent");
        }
    }
}
