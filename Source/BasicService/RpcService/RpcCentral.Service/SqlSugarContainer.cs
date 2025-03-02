using RpcCentral.Common;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.Service
{
    internal class SqlSugarContainer : IUnityContainer
    {
        private readonly IocBuffer _IocBuffer;
        public SqlSugarContainer (IocBuffer buffer)
        {
            this._IocBuffer = buffer;
        }
        public void Register (Type form, Type to)
        {
            this._IocBuffer.Register(form, to);
        }

        public void Register<Form, To> ()
        {
            this._IocBuffer.Register(typeof(Form), typeof(To));
        }

        public void RegisterSingle<T> (T to) where T : class
        {
            this._IocBuffer.RegisterSingle<T>(to);
        }

        public void RegisterSingle<Form, To> ()
        {
            this._IocBuffer.RegisterSingle(typeof(Form), typeof(To));
        }

        public T Resolve<T> ()
        {
            return UnityHelper.Resolve<T>();
        }
    }
}
