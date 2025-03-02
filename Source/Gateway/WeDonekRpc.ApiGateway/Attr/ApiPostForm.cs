using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    /// <summary>
    /// 标记此参数需通过PostForm的方式获取
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class ApiPostForm : Attribute
    {
        public ApiPostForm ()
        {
        }
        public ApiPostForm (string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
