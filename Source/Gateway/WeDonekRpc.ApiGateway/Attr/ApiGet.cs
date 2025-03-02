using System;

namespace WeDonekRpc.ApiGateway.Attr
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class ApiGet : Attribute
    {
        public ApiGet ()
        {
        }
        public ApiGet (string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
