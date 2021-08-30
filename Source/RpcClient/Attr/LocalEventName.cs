using System;

namespace RpcClient.Attr
{
        /// <summary>
        /// 本地事件名称
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public class LocalEventName : Attribute
        {
                public LocalEventName(string name)
                {
                        this.EventName = name;
                }
                public string EventName
                {
                        get;
                }
        }
}
