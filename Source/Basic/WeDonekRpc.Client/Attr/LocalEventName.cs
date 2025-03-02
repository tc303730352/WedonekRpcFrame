using System;

namespace WeDonekRpc.Client.Attr
{
    /// <summary>
    /// 本地事件名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LocalEventName : Attribute
    {

        public LocalEventName (params string[] names)
        {
            this.EventName = names;
        }
        public string[] EventName
        {
            get;
        }
    }
}
