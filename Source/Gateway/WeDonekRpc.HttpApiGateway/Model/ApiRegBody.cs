using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.Client.Ioc;
using System;

namespace WeDonekRpc.HttpApiGateway.Model
{
    internal class ApiRegBody
    {

        public ApiRegBody(IocBuffer ioc)
        {
            Ioc = ioc;
        }
        public IocBuffer Ioc { get; }

        public Type To { get; set; }

        public IApiModular Modular
        {
            get;
            set;
        }
       public  Action<IRoute> Action
        {
            get;
            set;
        }
    }
}
