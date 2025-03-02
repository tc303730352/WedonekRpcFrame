using System;

namespace WeDonekRpc.Client.Attr
{
    /// <summary>
    /// 指定IOC名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class IocName : Attribute
    {
        /// <summary>
        /// 自定义名称
        /// </summary>
        public string Name { get; }

        public IocName(string name)
        {
            this.Name = name;
        }
    }
}
