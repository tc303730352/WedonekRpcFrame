using Autofac;
using Autofac.Builder;
using WeDonekRpc.Helper;
using System.Reflection;
using WeDonekRpc.Helper.Lock;

namespace RpcCentral.Common
{
    public class IocBuffer : IDisposable
    {
        private List<string> _Assemplys = new List<string>();
        private LockHelper _lock = new LockHelper();
        private ContainerBuilder _builder;
        public IocBuffer()
        {
            _builder = new ContainerBuilder();
        }
        public void Init()
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies().FindAll(c => c.GetName().Name.StartsWith("RpcCentral."));
            assembly.ForEach(c =>
            {
                _Reg(c);
            });
        }
        public void Register(Type form, Type to, string name)
        {
            if (to.IsGenericType)
            {
                _builder.RegisterGeneric(to).As(form).InstancePerLifetimeScope();
            }
            else
            {
                _builder.RegisterType(to).Keyed(name, form).InstancePerLifetimeScope();
            }
        }
        public void Register(Type form, Type to)
        {
            if (to.IsGenericType)
            {
                _builder.RegisterGeneric(to).As(form).InstancePerLifetimeScope();
            }
            else
            {
                //InstancePerLifetimeScope
                _builder.RegisterType(to).InstancePerLifetimeScope().As(form);
            }
        }
        public void RegisterSingle<T>(T to) where T : class
        {
            _builder.RegisterInstance<T>(to).As(typeof(T)).SingleInstance();
        }
        public void RegisterSingle(Type form, Type to)
        {
            _builder.RegisterType(to).As(form).SingleInstance();
        }
        internal IContainer Build(ContainerBuildOptions options = ContainerBuildOptions.None)
        {
            return _builder.Build(options);
        }
        private void CurrentDomain_AssemblyLoad(object? sender, AssemblyLoadEventArgs args)
        {
            if (args.LoadedAssembly.GetName().Name.StartsWith("RpcCentral."))
            {
                _Reg(args.LoadedAssembly);
            }
        }
        private void _Reg(Assembly assembly)
        {
            if (_lock.GetLock())
            {
                if (_Assemplys.Contains(assembly.FullName))
                {
                    _lock.Exit();
                    return;
                }
                _Assemplys.Add(assembly.FullName);
                _lock.Exit();
            }
            Type[] types = assembly.GetTypes();
            Type[] list = types.FindAll(c => c.IsInterface && c.IsPublic);
            if (list.IsNull())
            {
                return;
            }
            list.ForEach(c =>
            {
                Type[] subs = types.FindAll(a => _CheckIsReg(a, c));
                if (subs.Length == 1)
                {
                    if (c.IsGenericType)
                    {
                        _builder.RegisterGeneric(subs[0]).As(c).InstancePerLifetimeScope();
                    }
                    else
                    {
                        _builder.RegisterType(subs[0]).As(c).InstancePerLifetimeScope();
                    }
                    _Init(subs[0]);
                }
            });
        }

        private static bool _CheckIsReg(Type type, Type form)
        {
            if (type.IsValueType || type.IsAbstract || !type.IsClass)
            {
                return false;
            }
            else if (!type.GetInterfaces().IsExists(a => a.FullName == form.FullName))
            {
                return false;
            }
            else if (type.IsGenericType != form.IsGenericType)
            {
                return false;
            }
            else if (type.IsGenericType)
            {
                Type[] toG = type.GetGenericArguments();
                Type[] fG = form.GetGenericArguments();
                if (!toG.IsEquals(fG))
                {
                    return false;
                }
            }
            ConstructorInfo[] list = type.GetConstructors().FindAll(a => a.IsPublic && _CheckIsReg(a));
            return list.Length != 0;
        }
        private static bool _CheckIsReg(ConstructorInfo constructor)
        {
            ParameterInfo[] param = constructor.GetParameters();
            if (param.Length == 0)
            {
                return true;
            }
            return param.TrueForAll(a => a.ParameterType.IsInterface);
        }
        private void _Init(Type type)
        {
            ConstructorInfo[] list = type.GetConstructors();
            list.ForEach(c =>
            {
                ParameterInfo[] ps = c.GetParameters();
                if (ps.Length > 0 && ps.TrueForAll(a => a.ParameterType.IsInterface))
                {
                    ps.ForEach(a =>
                    {
                        _Reg(a.ParameterType.Assembly);
                    });
                }
            });
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomain_AssemblyLoad;
            _lock.Dispose();
            _lock = null;
            _Assemplys.Clear();
            _Assemplys = null;
            _builder = null;
        }
    }
}
