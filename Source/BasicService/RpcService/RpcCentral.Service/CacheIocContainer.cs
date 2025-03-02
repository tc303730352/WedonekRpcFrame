using RpcCentral.Common;
using WeDonekRpc.CacheClient.Interface;

namespace RpcCentral.Service
{
    internal class CacheIocContainer : IIocContainer
    {
        private readonly IocBuffer _Ioc;
        public CacheIocContainer (IocBuffer ioc)
        {
            this._Ioc = ioc;
        }
        public void Register (Type form, Type to)
        {
            this._Ioc.Register(form, to);
        }

        public void Register<Form, To> ()
        {
            this._Ioc.Register(typeof(Form), typeof(To));
        }

        public void RegisterSingle<T> (T to) where T : class
        {
            this._Ioc.RegisterSingle<T>(to);
        }

        public void RegisterSingle<Form, To> ()
        {
            this._Ioc.RegisterSingle(typeof(Form), typeof(To));
        }
    }
}
