using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

using RpcClient.Attr;
using RpcClient.EventBus;
using RpcClient.Interface;

using RpcHelper;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class LocalEventCollect : ILocalEventCollect
        {
                private static readonly Type _AttrType = typeof(Attr.LocalEventName);
                private static readonly ConcurrentDictionary<string, LocalEvent> _Events = new ConcurrentDictionary<string, LocalEvent>();

                public bool RegEvent<T>(IEventHandler<T> handler)
                {
                        return this._RegEvent(typeof(IEventHandler<T>), handler.GetType());
                }
                private string _GetEventName(Type type)
                {
                        LocalEventName attr = (LocalEventName)type.GetCustomAttribute(_AttrType);
                        if (attr == null)
                        {
                                return null;
                        }
                        return attr.EventName;
                }
                private string _GetEventName(Type type, string name)
                {
                        if (name == null)
                        {
                                return type.FullName;
                        }
                        return string.Concat(type.FullName, "_", name);
                }
                public void RemoveEvent<T>(string name = null)
                {
                        this._RemoveEvent(typeof(T), name);
                }
                private void _RemoveEvent(Type type, string name)
                {
                        string key = this._GetEventName(type, name);
                        if (_Events.TryGetValue(key, out LocalEvent local))
                        {
                                local.Remove(type.FullName);
                        }
                }

                public void Public(object eventData, string name = null)
                {
                        Type type = eventData.GetType();
                        string key = this._GetEventName(type, name);
                        if (_Events.TryGetValue(key, out LocalEvent local))
                        {
                                local.Post(eventData);
                        }
                }

                private bool _RegEvent(Type form, Type to)
                {
                        string name = _GetEventName(to);
                        Type elem = form.GenericTypeArguments[0];
                        LocalEvent local = this._GetLocalEvent(elem, name);
                        RpcClient.Unity.RegisterType(form, to, to.FullName);
                        return local.Reg(form, to.FullName);
                }
                public void RegLocalEvent(Assembly assembly)
                {
                        if(assembly.GetName().Name== "Wedonek.Demo.Service")
                        {

                        }
                        assembly.GetTypes().ForEach(a =>
                        {
                                if (!a.IsClass)
                                {
                                        return;
                                }
                                Type[] types = a.GetInterfaces();
                                if (types.IsNull())
                                {
                                        return;
                                }
                                Type form = types.Find(b => b.Namespace == "RpcClient.Interface" && b.IsGenericType && b.Name == "IEventHandler`1");
                                if (form == null)
                                {
                                        return;
                                }
                                this._RegEvent(form, a);
                        });
                }

                private LocalEvent _GetLocalEvent(Type type, string name)
                {
                        string key = _GetEventName(type, name);
                        if(name == null)
                        {
                                name = type.Name;
                        }
                        return _Events.GetOrAdd(key, new LocalEvent(name));
                }



                public void AsyncPublic(object eventData, string name = null)
                {
                        Type type = eventData.GetType();
                        string key = this._GetEventName(type, name);
                        if (_Events.TryGetValue(key, out LocalEvent local))
                        {
                                local.AsyncPost(eventData);
                        }
                }
        }
}
