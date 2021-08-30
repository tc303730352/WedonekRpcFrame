using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using RpcClient.Interface;

using RpcHelper;

namespace RpcClient.EventBus
{
        internal class LocalEvent
        {
                private readonly ConcurrentDictionary<string, ILocalHandler> _Handlers = new ConcurrentDictionary<string, ILocalHandler>();
                private event Action<object, string> _HandleEvent = null;

                public string EventName
                {
                        get;
                }
                public LocalEvent(string name)
                {
                        this.EventName = name;
                }

                public void Post(object arg)
                {
                        if (_HandleEvent != null)
                        {
                                _ExecAction(arg);
                        }
                }
                public void AsyncPost(object arg)
                {
                        if (_HandleEvent != null)
                        {
                                Task.Run(() =>
                                {
                                        _ExecAction(arg);
                                });
                        }
                }
                private void _ExecAction(object arg)
                {
                        try
                        {
                                _HandleEvent(arg, this.EventName);
                        }
                        catch (Exception e)
                        {
                                ErrorException error = ErrorException.FormatError(e);
                                error.SaveLog(arg.ToJson(), "LocalEvent");
                        }
                }

                public bool Reg(Type type, string name)
                {
                        if (this._Handlers.ContainsKey(name))
                        {
                                return false;
                        }
                        ILocalHandler handler = new LocalEventHandler(type, name);
                        if (this._Handlers.TryAdd(name, handler))
                        {
                                _HandleEvent += handler.HandleEvent;
                                return true;
                        }
                        return false;
                }

                public void Remove(string name)
                {
                        this._Handlers.TryRemove(name, out _);
                }
        }
}
