using System;

namespace WeDonekRpc.ApiGateway.Modular
{
    public class ApiBody
    {
        public Type Form
        {
            get;
            set;
        }
        public Type To
        {
            get;
            set;
        }
        public string Name { get;  set; }
    }
}
