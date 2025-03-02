using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Modular;

namespace WeDonekRpc.ApiGateway.PlugIn
{
    /// <summary>
    /// 接口屏蔽插件
    /// </summary>
    internal class ShieIdPlugIn : BasicPlugIn, IShieIdPlugIn
    {
        private readonly IResourceShieldService _Service;
        private readonly IApiShieldConfig _Config;
        public ShieIdPlugIn (IResourceShieldService service, IApiShieldConfig config) : base("ApiShieId")
        {
            this._Service = service;
            this._Config = config;
            config.AddRefreshEvent(this._Refresh);
        }
        public override bool IsEnable => this._Config.IsEnable;
        private void _Init ()
        {
            if (!this._Config.IsEnable)
            {
                this._Service.Dispose();
            }
            else
            {
                this._Service.Init();
            }
        }
        private void _Refresh (string name)
        {
            this._Init();
            if (name != null)
            {
                base._ChangeEvent();
            }
        }

        protected override void _Dispose ()
        {
            if (this._Config.IsEnable)
            {
                this._Service.Dispose();
            }
        }

        public bool CheckIsShieId (string path)
        {
            if (this._Config.IsLocal)
            {
                return this._Config.ShieIdPath.IsExists(path);
            }
            return this._Service.CheckIsShieId(path);
        }
    }
}
