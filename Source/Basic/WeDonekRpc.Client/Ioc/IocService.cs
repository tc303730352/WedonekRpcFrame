using System;
using System.Threading;
using Autofac;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.Client.Ioc
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class IocService : IIocService
    {
        private static Autofac.IContainer _Container = null;
        private static readonly AsyncLocal<IocScope> _CurrentScope = new AsyncLocal<IocScope>(_ScopeChange);

        private static void _ScopeChange ( AsyncLocalValueChangedArgs<IocScope> e )
        {
            if ( e.CurrentValue != null && e.CurrentValue.IsDispose )
            {
                _CurrentScope.Value = null;
            }
        }
        internal static void Init ( IocBuffer buffer )
        {
            _Container = buffer.Build();
        }

        public bool IsRegistered ( Type type )
        {
            return _Container.IsRegistered(type);
        }
        public bool IsRegistered ( Type type, string name )
        {
            return _Container.IsRegisteredWithKey(name, type);
        }
        public IocScope CreateTempScore ()
        {
            ILifetimeScope lifetime = _Container.BeginLifetimeScope();
            return new IocScope(lifetime);
        }
        public IocScope CreateScore ()
        {
            ILifetimeScope lifetime = _Container.BeginLifetimeScope();
            _CurrentScope.Value = new IocScope(lifetime, _Disposable);
            return _CurrentScope.Value;
        }
        private static void _Disposable ( ILifetimeScope scope )
        {
            _CurrentScope.Value = null;
        }
        public IocScope CreateScore ( object key )
        {
            if ( _CurrentScope.Value == null || _CurrentScope.Value.IsDispose )
            {
                ILifetimeScope lifetime = _Container.BeginLifetimeScope(key);
                _CurrentScope.Value = new IocScope(lifetime, _Disposable);
                return _CurrentScope.Value;
            }
            else
            {
                return _CurrentScope.Value.CreateScore(key);
            }
        }
        public object Resolve ( Type form )
        {
            if ( _CurrentScope.Value == null )
            {
                return _Container.Resolve(form);
            }
            return _CurrentScope.Value.Resolve(form);
        }

        public object Resolve ( Type form, string name )
        {
            if ( _CurrentScope.Value == null )
            {
                return _Container.ResolveKeyed(name, form);
            }
            return _CurrentScope.Value.Resolve(form, name);
        }

        public T Resolve<T> ()
        {
            if ( _CurrentScope.Value != null )
            {
                return _CurrentScope.Value.Resolve<T>();
            }
            return _Container.Resolve<T>();
        }

        public T Resolve<T> ( string name )
        {
            if ( _CurrentScope.Value != null )
            {
                return _CurrentScope.Value.Resolve<T>(name);
            }
            return _Container.ResolveKeyed<T>(name);
        }

        public bool TryResolve<T> ( out T data ) where T : class
        {
            if ( _CurrentScope.Value != null )
            {
                return _CurrentScope.Value.TryResolve<T>(out data);
            }
            return _Container.TryResolve<T>(out data);
        }

        public bool TryResolve<T> ( string name, out T data ) where T : class
        {
            if ( _CurrentScope.Value != null )
            {
                return _CurrentScope.Value.TryResolve<T>(name, out data);
            }
            return _Container.TryResolveKeyed<T>(name, out data);
        }
    }
}
