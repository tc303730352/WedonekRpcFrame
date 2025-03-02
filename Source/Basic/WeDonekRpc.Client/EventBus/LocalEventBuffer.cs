using System;
using System.Collections.Generic;
using System.Reflection;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.EventBus
{
    public class LocalEventBuffer
    {
        private readonly IocBuffer _Ioc;
        private readonly Type _AttrType = typeof(LocalEventName);
        private readonly Dictionary<string, LocalEvent> _Events = [];

        internal LocalEventBuffer (IocBuffer Ioc)
        {
            this._Ioc = Ioc;
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T">事件源</typeparam>
        /// <param name="action">事件委托</param>
        /// <param name="name">事件名称</param>
        /// <returns>是否成功</returns>
        public void Reg<T> (LocalEvent<T> action, string name)
        {
            this._RegEvent(action, name);
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T">事件源</typeparam>
        /// <param name="action">事件委托</param>
        /// <returns>是否成功</returns>
        public void Reg<T> (LocalEvent<T> action)
        {
            this._RegEvent(action, string.Empty);
        }
        public void Reg (Type type)
        {
            if (!type.IsClass || type.IsIgnore())
            {
                return;
            }
            Type[] inters = type.GetInterfaces();
            Type form = inters.Find(b => b.Namespace == "WeDonekRpc.Client.Interface" && b.IsGenericType && b.Name == "IEventHandler`1");
            if (form != null)
            {
                if (this._Ioc.Register(new IocBody(form, type, type.FullName, 10)))
                {
                    this._RegEvent(form, type);
                }
                return;
            }
            form = inters.Find(b => b.Namespace == "WeDonekRpc.Client.Interface" && b.IsGenericType && b.Name == "ILocalComplateEvent`1");
            if (form != null)
            {
                if (this._Ioc.Register(new IocBody(form, type, type.FullName, 10)))
                {
                    this._RegComplteEvent(form, type);
                }
                return;
            }
        }


        private void _RegEvent<T> (LocalEvent<T> action, string name)
        {
            LocalEventDelegate<T> dele = new EventBus.LocalEventDelegate<T>(action);
            Type to = action.GetType();
            IocBody body = this._Ioc.RegisterInstance<IEventHandler<T>, LocalEventDelegate<T>>(dele, to.FullName);
            if (body != null)
            {
                LocalEvent local = this._GetLocalEvent(typeof(T), name);
                local.Reg(typeof(IEventHandler<T>), body.Name);
            }
        }
        /// <summary>
        /// 获取事件名称
        /// </summary>
        /// <param name="type">事件源类型</param>
        /// <param name="name">事件名</param>
        /// <returns>事件名</returns>
        private string _GetEventName (Type type, string name)
        {
            if (name == null)
            {
                return type.FullName;
            }
            return string.Concat(type.FullName, "_", name);
        }
        private string[] _GetEventName (Type type)
        {
            LocalEventName attr = (LocalEventName)type.GetCustomAttribute(this._AttrType);
            if (attr == null)
            {
                return null;
            }
            return attr.EventName;
        }


        private LocalEvent _GetLocalEvent (Type type, string name)
        {
            string key = this._GetEventName(type, name);
            if (key.IsNull())
            {
                return null;
            }
            if (name == null)
            {
                name = type.Name;
            }
            return this._Events.GetOrAdd(key, a => new LocalEvent(name));
        }
        private void _RegComplteEvent (Type form, Type to)
        {
            string[] name = this._GetEventName(to);
            Type elem = form.GenericTypeArguments[0];
            if (name.IsNull())
            {
                LocalEvent local = this._GetLocalEvent(elem, null);
                local?.RegCompleteEvent(form, to.FullName);
            }
            else
            {
                name.ForEach(c =>
                {
                    LocalEvent local = this._GetLocalEvent(elem, c);
                    local.RegCompleteEvent(form, to.FullName);
                });
            }
        }
        private void _RegEvent (Type form, Type to)
        {
            string[] name = this._GetEventName(to);
            Type elem = form.GenericTypeArguments[0];
            if (name.IsNull())
            {
                LocalEvent local = this._GetLocalEvent(elem, null);
                local?.Reg(form, to.FullName);
            }
            else
            {
                name.ForEach(c =>
                {
                    LocalEvent local = this._GetLocalEvent(elem, c);
                    local.Reg(form, to.FullName);
                });
            }
        }

        internal Dictionary<string, LocalEvent> Build ()
        {
            return this._Events;
        }

    }
}
