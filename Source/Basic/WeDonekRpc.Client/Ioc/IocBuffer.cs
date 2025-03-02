using Autofac;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Reflection;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Ioc
{
    public delegate void InitIocBody (IocBody body);

    public class IocBuffer : IDisposable
    {
        private const int _DefSort = 10;
        private readonly List<Type> _Repeats = new List<Type>(20);
        private readonly Dictionary<string, IocBody> _IocCache = new Dictionary<string, IocBody>(200);
        private readonly List<string> _Assemblys = new List<string>(20);
        private ClassLifetimeType _DefLifetime = ClassLifetimeType.Scope;
        private event InitIocBody _Def = null;
        private readonly Action<Type[], string> _RegEvent;
        internal IocBuffer (Action<Type[], string> regEvent)
        {
            this._RegEvent = regEvent;
        }
        private void _InitBody (IocBody body)
        {
            if (this._Def != null)
            {
                this._Def(body);
            }
            if (!body.LifetimeType.HasValue)
            {
                body.SetLifetimeType(this._DefLifetime);
            }
        }
        internal Action<Type[]> RegEvent;
        private bool _AddAssembly (string name)
        {
            if (this._Assemblys.Contains(name))
            {
                return false;
            }
            this._Assemblys.Add(name);
            return true;
        }
        public void Load (Assembly assembly, int sort = _DefSort)
        {
            string name = assembly.GetName().Name;
            if (this._AddAssembly(name))
            {
                Type[] types = assembly.GetTypes();
                this.Load(types, sort);
                this._RegEvent(types, name);
            }
        }

        public void SetDefLifetimeType (InitIocBody func)
        {
            this._Def += func;
        }
        public void SetDefLifetimeType (ClassLifetimeType type)
        {
            this._DefLifetime = type;
        }
        public IocBody Find (Type form, Type to)
        {
            IocBody body = new IocBody(form, to, 0);
            if (!body.Check())
            {
                return null;
            }
            body.InitBody(this._InitBody);
            if (this._IocCache.TryGetValue(body.Key, out body))
            {
                return body;
            }
            return null;
        }
        public IocBody Register (Type form, Type to, int sort = _DefSort)
        {
            IocBody body = this.RegisterType(form, to, sort);
            if (body != null)
            {
                this.Register(to);
            }
            return body;
        }
        public IocBody Register (Type form, Type to, string name, int sort = _DefSort)
        {
            IocBody body = this.RegisterType(form, to, name, sort);
            if (body != null)
            {
                this.Register(to);
            }
            return body;
        }
        private bool _Add (IocBody body)
        {
            if (this._IocCache.TryGetValue(body.Key, out IocBody ioc))
            {
                if (body.Sort > ioc.Sort)
                {
                    this._IocCache[body.Key] = body;
                }
                return false;
            }
            this._IocCache.Add(body.Key, body);
            return true;
        }
        /// <summary>
        /// 注册类型（递归扫描该类型下的所有构造函数类型自动注册）
        /// </summary>
        /// <param name="type"></param>
        public void Register (Type type, int sort = _DefSort)
        {
            if (this._Repeats.Contains(type))
            {
                return;
            }
            this._Repeats.Add(type);
            if (type.IsInterface)
            {
                this.LoadType(type, sort);
            }
            else if (this.RegConstructors(type))
            {
                this.LoadType(type);
            }
        }
        private void _RegisterGeneric (IocBody body, ContainerBuilder container)
        {
            if (body.LifetimeType == ClassLifetimeType.InstancePerOwned)
            {
                IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> builder = container.RegisterGeneric(body.To).InstancePerOwned(body.Parent);
                if (body.Name.IsNotNull())
                {

                    _ = builder.Keyed(body.Name, body.Form);
                }
                else
                {
                    _ = builder.As(body.Form);
                }
            }
            else
            {
                IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> builder = container.RegisterGeneric(body.To);
                if (body.Name.IsNotNull())
                {
                    builder = builder.Keyed(body.Name, body.Form);
                }
                else
                {
                    builder = builder.As(body.Form);
                }
                switch (body.LifetimeType)
                {
                    case ClassLifetimeType.Scope:
                        _ = builder.InstancePerLifetimeScope();
                        break;
                    case ClassLifetimeType.SingleInstance:
                        _ = builder.SingleInstance();
                        break;
                    case ClassLifetimeType.InstancePerMatchingLifetimeScope:
                        _ = builder.InstancePerMatchingLifetimeScope();
                        break;
                    default:
                        _ = builder.InstancePerDependency();
                        break;
                }
            }
        }
        public bool Register (IocBody body)
        {
            if (!body.Check())
            {
                return false;
            }
            body.InitBody(this._InitBody);
            if (!this._Add(body))
            {
                return false;
            }
            this.RegConstructor(body.To);
            return true;
        }
        private void _Add (IocBody body, ContainerBuilder container)
        {
            if (body.LifetimeType == ClassLifetimeType.SingleInstance && body.Source != null)
            {
                IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> builder = container.RegisterInstance(body.Source);
                if (body.Name.IsNotNull())
                {
                    _ = builder.Keyed(body.Name, body.Form).SingleInstance();
                }
                else
                {
                    _ = builder.As(body.Form).SingleInstance();
                }
            }
            else if (body.To.IsGenericType)
            {
                this._RegisterGeneric(body, container);
            }
            else if (body.LifetimeType == ClassLifetimeType.InstancePerOwned)
            {
                IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder = container.RegisterType(body.To).InstancePerOwned(body.Parent);
                if (body.Name.IsNotNull())
                {
                    _ = builder.Keyed(body.Name, body.Form);
                }
                else
                {
                    _ = builder.As(body.Form);
                }
            }
            else
            {
                IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder = container.RegisterType(body.To);
                if (body.Name.IsNotNull())
                {
                    builder = builder.Keyed(body.Name, body.Form);
                }
                else
                {
                    builder = builder.As(body.Form);
                }
                switch (body.LifetimeType)
                {
                    case ClassLifetimeType.Scope:
                        _ = builder.InstancePerLifetimeScope();
                        break;
                    case ClassLifetimeType.SingleInstance:
                        _ = builder.SingleInstance();
                        break;
                    case ClassLifetimeType.InstancePerMatchingLifetimeScope:
                        _ = builder.InstancePerMatchingLifetimeScope();
                        break;
                    default:
                        _ = builder.InstancePerDependency();
                        break;
                }
            }
        }

        public IocBody RegisterType (Type form, Type to, int sort = _DefSort)
        {
            IocBody body = new IocBody(form, to, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterType<T> (Type to, int sort = _DefSort)
        {
            IocBody body = new IocBody(typeof(T), to, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterType<T> (Type to, string name, int sort = _DefSort)
        {
            IocBody body = new IocBody(typeof(T), to, name, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        public IocBody RegisterInstance<Inter, T> (T data, int sort = _DefSort) where T : class, Inter
        {
            if (data == null)
            {
                return null;
            }
            IocBody body = new IocBody(typeof(Inter), data.GetType(), data, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterInstance<T> (T data, string name, int sort = _DefSort) where T : class
        {
            if (data == null)
            {
                return null;
            }
            IocBody body = new IocBody(typeof(T), data.GetType(), data, name, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterInstance<T> (T data, int sort = _DefSort) where T : class
        {
            if (data == null)
            {
                return null;
            }
            IocBody body = new IocBody(typeof(T), data.GetType(), data, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        public IocBody RegisterInstance<Inter, T> (T data, string name, int sort = _DefSort) where T : class, Inter
        {
            if (data == null)
            {
                return null;
            }
            IocBody body = new IocBody(typeof(Inter), data.GetType(), data, name, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterInstance<T> (T data, Type form, string name, int sort = _DefSort) where T : class
        {
            if (data == null)
            {
                return null;
            }
            IocBody body = new IocBody(form, data.GetType(), data, name, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterInstance<T> (T data, Type form, int sort = _DefSort) where T : class
        {
            if (data == null)
            {
                return null;
            }
            IocBody body = new IocBody(form, data.GetType(), data, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        public IocBody RegisterInstance (Type form, Type to, string name, int sort = _DefSort)
        {
            IocBody body = new IocBody(form, to, name, ClassLifetimeType.SingleInstance, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterInstance (Type form, Type to, int sort = _DefSort)
        {
            IocBody body = new IocBody(form, to, ClassLifetimeType.SingleInstance, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }
        public IocBody RegisterType (Type form, Type to, string name, int sort = _DefSort)
        {
            IocBody body = new IocBody(form, to, name, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        public IocBody RegisterType (Type form, Type to, ClassLifetimeType lifetimeType, int sort = _DefSort)
        {
            IocBody body = new IocBody(form, to, lifetimeType, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        public IocBody RegisterType (Type form, Type to, string name, ClassLifetimeType lifetimeType, int sort)
        {
            IocBody body = new IocBody(form, to, name, lifetimeType, sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        public IocBody RegisterType<Form, To> (int sort = _DefSort)
        {
            IocBody body = new IocBody(typeof(Form), typeof(To), sort);
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        public IocBody RegisterType<Form, To> (ClassLifetimeType lifetimeType, int sort = _DefSort)
        {
            IocBody body = new IocBody(typeof(Form), typeof(To), lifetimeType, sort)
            {
                Sort = 10
            };
            if (!this.Register(body))
            {
                return null;
            }
            return body;
        }

        internal Autofac.IContainer Build (ContainerBuildOptions options = ContainerBuildOptions.None)
        {
            ContainerBuilder builder = new ContainerBuilder();
            this._IocCache.ForEach((key, val) =>
            {
                this._Add(val, builder);
            });
            return builder.Build(options);
        }

        public void Dispose ()
        {
            this._Repeats.Clear();
            this._Assemblys.Clear();
            this._IocCache.Clear();
        }
    }
}
