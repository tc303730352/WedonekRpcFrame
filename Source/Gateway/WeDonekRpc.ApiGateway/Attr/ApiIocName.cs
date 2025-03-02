using System;

namespace WeDonekRpc.ApiGateway.Attr
{
        /// <summary>
        /// 设定方法体参数中接口对应IOC容器中的名称
        /// </summary>
        [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
        public class ApiIocName : Attribute
        {
                public ApiIocName(string name)
                {
                        this.Name = name;
                }
                public string Name
                {
                        get;
                        set;
                }
        }
}
