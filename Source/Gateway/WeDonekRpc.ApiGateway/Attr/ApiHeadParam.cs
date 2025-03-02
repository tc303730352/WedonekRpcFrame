using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class ApiHeadParam : Attribute
    {
        public ApiHeadParam ()
        {
        }
        public ApiHeadParam (string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
