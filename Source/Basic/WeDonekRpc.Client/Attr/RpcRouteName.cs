using System;

namespace WeDonekRpc.Client.Attr
{
    /// <summary>
    /// 自定义名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RpcRouteName : Attribute
    {
        public RpcRouteName(string name)
        {
            this.RouteName = name;
        }
        public string RouteName
        {
            get;
        }
    }
}
