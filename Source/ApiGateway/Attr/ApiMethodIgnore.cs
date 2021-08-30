using System;

namespace ApiGateway.Attr
{
        /// <summary>
        /// 方法忽略
        /// </summary>
        [AttributeUsage(AttributeTargets.Method, Inherited = true)]
        public class ApiMethodIgnore : Attribute
        {
        }
}
