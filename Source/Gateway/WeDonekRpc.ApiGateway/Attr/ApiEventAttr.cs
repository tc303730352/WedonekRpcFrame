using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class ApiEventAttr : Attribute
    {
        public Type ServiceEventType { get; private set; }
        public ApiEventAttr (Type serviceEventType)
        {
            this.ServiceEventType = serviceEventType;
        }
    }
}
