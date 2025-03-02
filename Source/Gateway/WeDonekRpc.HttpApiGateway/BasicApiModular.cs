using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.Config;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Service;

namespace WeDonekRpc.HttpApiGateway
{
    /// <summary>
    /// 模块的基类
    /// </summary>
    public class BasicApiModular : IApiModular
    {
        private HttpGatewayOption _Option;
        private readonly ModularConfig _Config;
        private bool _IsInit = false;
        public BasicApiModular (string name, string show = null)
        {
            this._Config = new ModularConfig();
            this.ServiceName = name;
            this.Route = new ModularRouteService(this);
            this.Show = show;
        }

        public IModularConfig Config => this._Config;
        public string ServiceName
        {
            get;
        }
        public bool IsInit => this._IsInit;

        public string ApiRouteFormat => this._Config.ApiRouteFormat;

        public IModularRouteService Route { get; }
        public string Show { get; }

        public void Load (IGatewayOption option)
        {
            ApiModularCollect.RegModular(this);
            this._Option = new HttpGatewayOption(option, this);
            this.Load(this._Option, this._Config);
            this._Option.Load();
        }

        public void InitModular ()
        {
            this._Option.Init();
            this._Option = null;
            this._IsInit = true;
            this.Init();
        }

        protected virtual void Load (IHttpGatewayOption option, IModularConfig config)
        {

        }
        protected virtual void Init ()
        {

        }

        public void Start ()
        {
        }

        public void Dispose ()
        {
        }

        public virtual void InitRouteConfig (IApiModel config)
        {

        }
    }
}
