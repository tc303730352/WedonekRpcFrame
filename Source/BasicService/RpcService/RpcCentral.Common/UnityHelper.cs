using Autofac;

namespace RpcCentral.Common
{
    public class UnityHelper
    {
        private static Autofac.IContainer _Container;
        private static readonly AsyncLocal<IocScope> _CurrentScope = new AsyncLocal<IocScope>();

        public static void Init (Action<IocBuffer> initAction)
        {
            using (IocBuffer buffer = new IocBuffer())
            {
                buffer.Init();
                initAction(buffer);
                _Container = buffer.Build();
            }
        }
        public static IocScope CreateTempScore ()
        {
            ILifetimeScope lifetime = _Container.BeginLifetimeScope();
            return new IocScope(lifetime, _Disposable);
        }
        public static IocScope CreateScore ()
        {
            if (_CurrentScope.Value == null || _CurrentScope.Value.IsDispose)
            {
                ILifetimeScope lifetime = _Container.BeginLifetimeScope();
                _CurrentScope.Value = new IocScope(lifetime, _Disposable);
                return _CurrentScope.Value;
            }
            else
            {
                return _CurrentScope.Value.CreateScore();
            }
        }
        private static void _Disposable (ILifetimeScope scope)
        {
            _CurrentScope.Value = null;
        }

        public static T Resolve<T> ()
        {
            if (_CurrentScope.Value == null)
            {
                return _Container.Resolve<T>();
            }
            return _CurrentScope.Value.Resolve<T>();
        }
        public static T Resolve<T> (string name)
        {
            if (_CurrentScope.Value == null)
            {
                return _Container.ResolveKeyed<T>(name);
            }
            return _CurrentScope.Value.Resolve<T>(name);
        }
    }
}
