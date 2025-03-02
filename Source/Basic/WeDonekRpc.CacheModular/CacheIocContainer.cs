using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;

namespace WeDonekRpc.CacheModular
{
    internal class CacheIocContainer : IIocContainer
    {
        private readonly IocBuffer _Ioc;
        public CacheIocContainer (RpcInitOption option)
        {
            this._Ioc = option.Ioc;
        }
        public void Register (Type form, Type to)
        {
            _ = this._Ioc.Register(form, to);
        }

        public void Register<Form, To> ()
        {
            _ = this._Ioc.Register(typeof(Form), typeof(To));
        }

        public void RegisterSingle<T> (T to) where T : class
        {
            _ = this._Ioc.RegisterInstance<T>(to);
        }

        public void RegisterSingle<Form, To> ()
        {
            _ = this._Ioc.RegisterInstance(typeof(Form), typeof(To));
        }
    }
}
