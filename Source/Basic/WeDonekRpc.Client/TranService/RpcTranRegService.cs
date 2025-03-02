using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Tran.Saga;
using WeDonekRpc.Client.Tran.Tcc;
using WeDonekRpc.Client.Tran.TwoPc;
using System;

namespace WeDonekRpc.Client.TranService
{
    internal class RpcTranRegService: IRpcTranRegService
    {
        private IocBuffer _Ioc;

        internal RpcTranRegService(IocBuffer Ioc)
        {
            _Ioc = Ioc;
        }

        public void RegSagaTran(string name, Action<ICurTran> rollback)
        {
            RpcTranService.RegTran(name, new SagaTranTemplate(name, rollback));
        }

        public void RegSagaTran<T>(Action<ICurTran> rollback) where T : class
        {
            string name = TranHelper.GetTranName(typeof(T));
            RegSagaTran(name, rollback);
        }

        public void RegTccTran<Ev>(string name) where Ev : IRpcTccEvent
        {
            IocBody body = this._Ioc.Register(typeof(IRpcTccEvent), typeof(Ev), name);
            if (body != null)
            {
                RpcTranService.RegTran(name, new TccTranTemplate(name));
            }
        }

        public void RegTccTran<T, Ev>()
            where T : class
            where Ev : IRpcTccEvent
        {
            string name = TranHelper.GetTranName(typeof(T));
            RegTccTran<Ev>(name);
        }

        public void RegTwoPcTran<Ev>(string name) where Ev : IRpcTwoPcTran
        {
            IocBody body = this._Ioc.Register(typeof(IRpcTwoPcTran), typeof(Ev), name);
            if (body != null)
            {
                RpcTranService.RegTran(name, new RpcPcTranTemplate(name));
            }
        }
        public void RegTwoPcTran<T,Ev>() where Ev : IRpcTwoPcTran where T : class
        {
            string name = TranHelper.GetTranName(typeof(T));
            RegTwoPcTran<Ev>(name);
        }
    }
}
