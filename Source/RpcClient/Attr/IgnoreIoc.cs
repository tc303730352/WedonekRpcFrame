using System;

namespace RpcClient.Attr
{
        /// <summary>
        /// 标注IOC应忽略该类
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
        public class IgnoreIoc : Attribute
        {
        }
}
