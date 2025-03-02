 using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.IpBlack;

namespace WeDonekRpc.ApiGateway.PlugIn
{
    /// <summary>
    /// IP黑名单
    /// </summary>
    internal class IpBlackListPlugIn : BasicPlugIn, IIpBlackListPlugIn
    {
        private IIpBlack _IpBack;

        private readonly IIpBlackConfig _Config;

        public override bool IsEnable => this._Config.IsEnable;

        public IpBlackListPlugIn (IIpBlackConfig config) : base("IpBack")
        {
            this._Config = config;
            config.AddRefreshEvent(this._Refresh);
        }
        protected override void _InitPlugIn ()
        {
            if (!this._Config.IsEnable)
            {
                return;
            }
            else if (!this._Config.IsLocal)
            {
                this._IpBack = new RemoteIpBack();
            }
            else
            {
                this._IpBack = new LocalIpBlack();
            }
            this._IpBack.Init(this._Config);
        }
        protected override void _Dispose ()
        {
            if (!this._Config.IsEnable)
            {
                this._IpBack?.Dispose();
                this._IpBack = null;
            }
        }
        private void _Refresh (string name)
        {
            if (name != null)
            {
                base._ChangeEvent();
            }
            if (this._Config.IsEnable && this._IpBack != null)
            {
                this._IpBack.Init(this._Config);
            }
        }

        public bool IsLimit (string ip)
        {
            if (!this._IpBack.IsInit)
            {
                return false;
            }
            return this._IpBack.IsLimit(ip);
        }
    }
}
