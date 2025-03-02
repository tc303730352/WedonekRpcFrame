using System;

namespace WeDonekRpc.Client.Attr
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RpcSubscribeName : Attribute
    {
        public RpcSubscribeName(string name)
        {
            this.RouteName = name;
        }
        public string RouteName
        {
            get;
        }
    }
}
