using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    /// <summary>
    /// 标记参数读取路由参数获取
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class ApiRouteParam : Attribute
    {
        public ApiRouteParam ()
        {
        }
        public ApiRouteParam (string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
