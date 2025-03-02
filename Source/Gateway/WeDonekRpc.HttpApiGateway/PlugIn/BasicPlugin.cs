using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    internal class BasicPlugin : IHttpPlugIn
    {
        private IPlugIn _Plugin = null;

        protected void _Init (IPlugIn plugIn)
        {
            this._Plugin = plugIn;
        }

        public string Name => this._Plugin.Name;

        public bool IsEnable => this._Plugin.IsEnable;

        public void Dispose ()
        {
            this._Plugin.Dispose(this._Refresh);
        }
        private void _Refresh (IPlugIn plugIn)
        {
            IPlugInService service = RpcClient.Ioc.Resolve<IPlugInService>();
            if (plugIn.IsEnable)
            {
                service.Add(this);
            }
        }
        public void Init ()
        {
            this._Plugin.Init(this._Refresh);
            this._InitPlugIn();
        }
        protected virtual void _InitPlugIn ()
        {

        }

        public virtual void Exec (IRoute route, IHttpHandler handler)
        {
        }
    }
}
