using Autofac;

namespace RpcCentral.Common
{
    public class IocScope : IDisposable
    {
        private ILifetimeScope _Lifetime;
        private readonly ILifetimeScope _Parent;
        private readonly Action<ILifetimeScope> _DisposableEvent;
        internal IocScope (ILifetimeScope lifetime, Action<ILifetimeScope> disposableEvent)
        {
            this._Lifetime = lifetime;
            this._DisposableEvent = disposableEvent;
        }
        internal IocScope (ILifetimeScope lifetime, ILifetimeScope parent, Action<ILifetimeScope> disposableEvent)
        {
            this._Parent = parent;
            this._Lifetime = lifetime;
            this._DisposableEvent = disposableEvent;
        }
        public IocScope CreateScore ()
        {
            ILifetimeScope lifetime = this._Lifetime.BeginLifetimeScope();
            return new IocScope(lifetime, this._Lifetime, new Action<ILifetimeScope>(this._rollback));
        }
        public bool IsDispose
        {
            get;
            private set;
        }
        private void _rollback (ILifetimeScope scope)
        {
            this._Lifetime = scope;
        }

        public T Resolve<T> ()
        {
            return this._Lifetime.Resolve<T>();
        }
        public T Resolve<T> (string name)
        {
            return this._Lifetime.ResolveKeyed<T>(name);
        }
        public void Dispose ()
        {
            this.IsDispose = true;
            this._Lifetime.Dispose();
            this._DisposableEvent(this._Parent);
        }
    }
}
