using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.Modular;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;

namespace WeDonekRpc.ApiGateway
{
    [IgnoreIoc]
    internal class GatewayOption : IGatewayOption
    {
        private readonly IocBuffer _Ioc;
        private readonly ModularBuffer _Modular;
        internal GatewayOption (RpcInitOption option)
        {
            this.Option = option;
            this._Ioc = option.Ioc;
            this._Modular = new ModularBuffer(this);
        }

        public IocBuffer IocBuffer => this._Ioc;

        public RpcInitOption Option { get; }

        public void RegDoc (IApiDocModular doc)
        {
            IocBody ioc = this._Ioc.RegisterInstance(doc);
            if (ioc != null)
            {
                this.RegModular(doc);
            }
        }

        public void RegModular (IModular modular)
        {
            this._Modular.Reg(modular);

        }
        internal void Init ()
        {
            this._Modular.Init();
        }
    }
}
