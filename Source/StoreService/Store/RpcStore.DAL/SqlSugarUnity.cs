using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL
{
    internal class SqlSugarUnity : IUnityContainer
    {
        private readonly IIocService _Unity;
        private readonly IocBuffer _Buffer;
        public SqlSugarUnity (IocBuffer buffer)
        {
            this._Buffer = buffer;
            this._Unity = RpcClient.Ioc;
        }
        public void Register (Type form, Type to)
        {
            _ = this._Buffer.RegisterType(form, to);
        }

        public void Register<Form, To> ()
        {
            _ = this._Buffer.RegisterType<Form, To>();
        }

        public void RegisterSingle<T> (T to) where T : class
        {
            _ = this._Buffer.RegisterInstance<T>(to);
        }

        public void RegisterSingle<Form, To> ()
        {
            _ = this._Buffer.RegisterType<Form, To>(ClassLifetimeType.SingleInstance);
        }

        public T Resolve<T> ()
        {
            return this._Unity.Resolve<T>();
        }
    }
}
