using System.Linq;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Modular.Shield
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RpcDirectShieldPlugIn : IRpcDirectShieldPlugIn
    {
        private readonly IRpcDirectShieIdService _Shield;
        private readonly IRpcShieldConfig _Config;
        private readonly IRpcService _RpcService;
        private bool _IsEnable = false;
        public RpcDirectShieldPlugIn (IRpcDirectShieIdService shield, IRpcShieldConfig config, IRpcService service)
        {
            this._RpcService = service;
            this._Shield = shield;
            this._Config = config;
            this._Config.AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IRpcShieldConfig config)
        {
            if (config.IsEnable && config.IsLocal)
            {
                this._IsEnable = !config.DirectList.IsNull();
            }
            else if (config.IsEnable)
            {
                this._Shield.Init();
                this._IsEnable = true;
            }
            else
            {
                this._IsEnable = false;
                this._Shield.Close();
            }

        }
        public void Init ()
        {
            this._Shield.Init();
            this._RpcService.ReceiveMsg += this._CheckIsShield;
            this._Config.AddRefreshEvent(this._Refresh);
        }

        private void _CheckIsShield (IMsg msg)
        {
            if (!this._IsEnable)
            {
                return;
            }
            else if (this._Config.IsLocal)
            {
                if (this._Config.DirectList.Contains(msg.MsgKey))
                {
                    throw new ErrorException("rpc.direct.already.shield");
                }
            }
            else if (this._Shield.CheckIsShieId(msg.MsgKey))
            {
                throw new ErrorException("rpc.direct.already.shield");
            }
        }

    }
}
