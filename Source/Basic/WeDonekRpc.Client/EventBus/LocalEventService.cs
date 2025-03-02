using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.Client.EventBus
{
    /// <summary>
    /// 本地事件集合
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class LocalEventService : ILocalEventCollect
    {
        /// <summary>
        /// 事件列表
        /// </summary>
        private static Dictionary<string, LocalEvent> _Events;


        internal static void InitLocalEvent (LocalEventBuffer buffer)
        {
            _Events = new Dictionary<string, LocalEvent>(buffer.Build());
        }

        /// <summary>
        /// 获取事件名称
        /// </summary>
        /// <param name="type">事件源类型</param>
        /// <param name="name">事件名</param>
        /// <returns>事件名</returns>
        private string _GetEventName (Type type, string name)
        {
            return string.Concat(type.FullName, "_", name);
        }
        private string _GetEventName (Type type)
        {
            return type.FullName;
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="eventData">事件数据</param>
        /// <param name="name">事件名</param>
        public void Public (object eventData, string name)
        {
            Type type = eventData.GetType();
            if (name != null)
            {
                string key = this._GetEventName(type, name);
                if (_Events.TryGetValue(key, out LocalEvent local))
                {
                    local.Post(eventData);
                    return;
                }
            }
            this._Public(type, eventData);
        }
        private void _Public (Type type, object eventData)
        {
            string key = this._GetEventName(type);
            if (_Events.TryGetValue(key, out LocalEvent local))
            {
                local.Post(eventData);
            }
        }
        private void _AsyncPublic (Type type, object eventData)
        {
            string key = this._GetEventName(type);
            if (_Events.TryGetValue(key, out LocalEvent local))
            {
                local.AsyncPost(eventData);
            }
        }
        public void AsyncPublic (object eventData, string name)
        {
            Type type = eventData.GetType();
            if (name == null)
            {
                this._AsyncPublic(type, eventData);
            }
            else
            {
                string key = this._GetEventName(type, name);
                if (_Events.TryGetValue(key, out LocalEvent local))
                {
                    local.AsyncPost(eventData);
                }
            }
        }
    }
}
