using System;

namespace RpcClient.Attr
{
        /// <summary>
        /// 路由标记
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public class RpcRouteGroup : Attribute
        {
        }
}
