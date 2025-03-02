using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class ApiStop : Attribute
    {
    }
}
