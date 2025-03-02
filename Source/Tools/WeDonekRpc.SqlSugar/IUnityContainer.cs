namespace WeDonekRpc.SqlSugar
{
    public interface IUnityContainer
    {
        void RegisterSingle<T>(T to) where T : class;
        void Register(Type form, Type to);
        void Register<Form,To>();

        void RegisterSingle<Form, To>();
        T Resolve<T>();
    }
}