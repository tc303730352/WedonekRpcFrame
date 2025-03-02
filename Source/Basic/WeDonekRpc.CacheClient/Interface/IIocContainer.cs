using System;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IIocContainer
    {
        void RegisterSingle<T> (T to) where T : class;
        void Register (Type form, Type to);
        void Register<Form, To> ();

        void RegisterSingle<Form, To> ();
    }
}
