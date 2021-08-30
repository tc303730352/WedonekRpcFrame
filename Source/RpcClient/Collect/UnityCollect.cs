using System;
using System.Reflection;

using RpcClient.Attr;
using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;

using RpcHelper;

using Unity;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(ClassLifetimeType.单例)]
        internal class UnityCollect : IUnityCollect
        {
                private static readonly Type _IContainer = typeof(IContainer);
                private static readonly Type _UnityName = typeof(UnityName);
                private static readonly Type _IgnoreType = typeof(IgnoreIoc);
                private static readonly IUnityContainer _Container = null;

                public event Register RegisterEvent;

                public event Registering Registering;

                static UnityCollect()
                {
                        _Container = new UnityContainer();
                }
                public bool Register(Type form, Type to)
                {
                        if (this.RegisterType(form, to))
                        {
                                this.Load(to.Assembly);
                                this.Register(to);
                                return true;
                        }
                        return false;
                }
                public bool Register(Type form, Type to, string name)
                {
                        if (this.RegisterType(form, to, name))
                        {
                                this.Load(to.Assembly);
                                this.Register(to);
                                return true;
                        }
                        return false;
                }
                public void Load(string assemblyName)
                {
                        Assembly[] assembly = AppDomain.CurrentDomain.GetAssemblies();
                        Assembly obj = assembly.Find(a => a.GetName().Name == assemblyName);
                        if (obj != null)
                        {
                                this.Load(obj);
                        }
                }
                public void Load(Assembly assembly, Type type)
                {
                        Type[] types = assembly.GetTypes();
                        Type[] classs = types.FindAll(a => a.IsClass);
                        this._Register(type, classs);
                }
                public void Load(Assembly assembly)
                {
                        Type[] types = assembly.GetTypes();
                        Type[] inters = types.FindAll(a => a.IsInterface);
                        Type[] classs = types.FindAll(a => a.IsClass);
                        inters.ForEach(a =>
                        {
                                this._Register(a, classs);
                        });
                }
                /// <summary>
                /// 注册类型（递归扫描该类型下的所有构造函数类型自动注册）
                /// </summary>
                /// <param name="type"></param>
                public void Register(Type type)
                {
                        ConstructorInfo[] argType = type.GetConstructors();
                        Type[] allClass = type.Assembly.GetTypes().FindAll(a => a.IsClass);
                        argType.ForEach(a =>
                        {
                                if (!a.IsPublic)
                                {
                                        return;
                                }
                                ParameterInfo[] param = a.GetParameters();
                                if (param.Length == 0)
                                {
                                        return;
                                }
                                param.ForEach(b => b.ParameterType.IsInterface, b =>
                                {
                                        if (b.ParameterType.Assembly.FullName == type.Assembly.FullName)
                                        {
                                                this._Register(b.ParameterType, allClass);
                                        }
                                        else
                                        {
                                                this._Register(b.ParameterType);
                                        }
                                });
                        });
                }

                private void _Register(Type type)
                {
                        Type[] allClass = type.Assembly.GetTypes().FindAll(a => a.IsClass);
                        this._Register(type, allClass);
                }

                private string _GetTypeName(Type to, string def = null)
                {
                        UnityName name = (UnityName)to.GetCustomAttribute(_UnityName);
                        if (name == null)
                        {
                                return def;
                        }
                        return name.Name;
                }
                private bool _Register(Type form, Type[] to, Type[] classs)
                {
                        if (to.Length == 1)
                        {
                                return this.RegisterType(form, to[0]);
                        }
                        else
                        {
                                Type[] parent = to.FindAll(b => b.BaseType.Name == "Object");
                                bool isrename = parent.Length > 1;
                                foreach (Type a in parent)
                                {
                                        Type cur = a;
                                        string name = this._GetTypeName(a, a.Name);
                                        do
                                        {
                                                Type sub = classs.Find(b => b.BaseType.Name == cur.Name);
                                                if (sub == null)
                                                {
                                                        if (isrename)
                                                        {
                                                                //类型已注册 直接返回
                                                                if (!this.RegisterType(form, cur, name))
                                                                {
                                                                        return false;
                                                                }
                                                        }
                                                        else if (!this.RegisterType(form, cur))
                                                        {
                                                                return false;
                                                        }
                                                        break;
                                                }
                                                else if (!this.RegisterType(form, cur, name))
                                                {
                                                        return false;
                                                }
                                                cur = a;
                                        } while (true);
                                }
                                return true;
                        }
                }
                private void _Register(Type form, Type[] classs)
                {
                        Type[] list = classs.FindAll(c => c.GetInterface(form.Name) != null && c.GetCustomAttribute(_IgnoreType) == null);
                        if (list.Length == 0)
                        {
                                return;
                        }
                        else if (this._Register(form, list, classs))
                        {
                                list.ForEach(b => this.Register(b));
                        }
                }
                private bool _Registering(IocBody body)
                {
                        if (Registering != null)
                        {
                                return Registering(body);
                        }
                        return true;
                }
                private bool _IsRegistered(IocBody body)
                {
                        if (body.Name.IsNull())
                        {
                                return _Container.IsRegistered(body.Form);
                        }
                        return _Container.IsRegistered(body.Form, body.Name);
                }
                private bool _RegisterType(IocBody body)
                {
                        if (!this._Registering(body))
                        {
                                return false;
                        }
                        else if (this._IsRegistered(body))
                        {
                                return false;
                        }
                        if (!body.Name.IsNull())
                        {
                                _Container.RegisterType(body.Form, body.To, body.Name, UnityHelper.GetLifetime(body.Form, body.To));
                        }
                        else
                        {
                                _Container.RegisterType(body.Form, body.To, UnityHelper.GetLifetime(body.Form, body.To));
                        }
                        if (body.To.GetInterface(_IContainer.FullName) != null)
                        {
                                this._InitContainer(body.To);
                        }
                        RegisterEvent(body);
                        return true;
                }
                private bool _RegisterInstance<T>(IocBody body, T data)
                {
                        if (!this._Registering(body))
                        {
                                return false;
                        }
                        else if (this._IsRegistered(body))
                        {
                                return false;
                        }
                        if (!body.Name.IsNull())
                        {
                                _Container.RegisterInstance<T>(body.Name, data);
                        }
                        else
                        {
                                _Container.RegisterInstance<T>(data);
                        }
                        RegisterEvent(body);
                        if (body.To.GetInterface(_IContainer.FullName) != null)
                        {
                                this._InitContainer(body.To);
                        }
                        return true;
                }
                private IocBody _GetIocBody(Type form, Type to, string name)
                {
                        if (name.IsNull())
                        {
                                name = this._GetTypeName(to);
                        }
                        return new IocBody(form, to, name);
                }
                public bool RegisterType(Type form, Type to)
                {
                        return this._RegisterType(this._GetIocBody(form, to, null));
                }
                public bool RegisterType<T>(Type to)
                {
                        return this._RegisterType(this._GetIocBody(typeof(T), to, null));
                }
                public bool RegisterType<T>(Type to, string name)
                {
                        return this._RegisterType(this._GetIocBody(typeof(T), to, name));
                }
                /// <summary>
                /// 初始化化容器
                /// </summary>
                /// <param name="to"></param>
                private void _InitContainer(Type to)
                {
                        if (_Container.IsRegistered(_IContainer, to.FullName))
                        {
                                return;
                        }
                        IUnityContainer unity = _Container.RegisterInstance(_IContainer, to.FullName, to);
                        IContainer obj = (IContainer)unity.Resolve(_IContainer, to.FullName);
                        obj.InitContainer(this);
                }
                public bool RegisterInstance<T>(T data)
                {
                        return this._RegisterInstance<T>(this._GetIocBody(typeof(T), data.GetType(), null), data);
                }
                public bool RegisterInstance<T>(T data, string name)
                {
                        return this._RegisterInstance<T>(this._GetIocBody(typeof(T), data.GetType(), name), data);
                }
                public bool RegisterType(Type form, Type to, string name)
                {
                        return this._RegisterType(this._GetIocBody(form, to, name));
                }

                public object Resolve(Type form)
                {
                        return _Container.Resolve(form);
                }

                public object Resolve(Type form, string name)
                {
                        if(name == null)
                        {
                                return _Container.Resolve(form);
                        }
                        return _Container.Resolve(form, name);
                }

                public T Resolve<T>()
                {
                        return (T)this.Resolve(typeof(T));
                }

                public T Resolve<T>(string name)
                {
                        return (T)this.Resolve(typeof(T), name);
                }

                public void Load(Assembly assembly, Assembly Interfaces)
                {
                        Type[] types = Interfaces.GetTypes();
                        Type[] inters = types.FindAll(a => a.IsInterface);
                        types = assembly.GetTypes();
                        Type[] classs = types.FindAll(a => a.IsClass);
                        inters.ForEach(a =>
                        {
                                this._Register(a, classs);
                        });
                }
        }
}
