using System;

namespace RpcClient.Attr
{
        /// <summary>
        /// 自定义名称
        /// </summary>
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class RpcTcpRouteAttr : Attribute
        {
                public RpcTcpRouteAttr(string name)
                {
                        this.RouteName = name;
                }
                public string RouteName
                {
                        get;
                }
        }
}
