using System;

namespace WeDonekRpc.Client.Attr
{
        /// <summary>
        /// 标注IOC应忽略该类
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
        public class IgnoreIoc : Attribute
        {
        }
}
