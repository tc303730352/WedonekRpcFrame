using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class ApiPathParam : Attribute
    {
        public ApiPathParam ()
        {
        }
        public ApiPathParam (string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
