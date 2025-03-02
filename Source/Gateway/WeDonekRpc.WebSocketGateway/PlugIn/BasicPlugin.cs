using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.PlugIn
{
    internal class BasicPlugin : IWebSocketPlugin
    {
        private IPlugIn _Plugin = null;

        protected void _Init (IPlugIn plugIn, ExecStage pluginType)
        {
            this.ExecStage = pluginType;
            this._Plugin = plugIn;
        }
        public ExecStage ExecStage { get; private set; }
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
        public virtual bool Exec (IApiService service, ApiHandler handler, out string error)
        {
            error = null;
            return true;
        }
        public virtual bool RequestInit (IApiService service, out string error)
        {
            error = null;
            return true;
        }
        public virtual bool Authorize (RequestBody request)
        {
            return true;
        }
        public override bool Equals (object obj)
        {
            if (obj is IWebSocketPlugin i)
            {
                return i.Name == this.Name;
            }
            return false;
        }
        public override int GetHashCode ()
        {
            return this.Name.GetHashCode();
        }
        public bool Equals (IWebSocketPlugin other)
        {
            return other.Name == this.Name;
        }
    }
}
