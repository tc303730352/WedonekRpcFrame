using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Error;

namespace RpcSync.Service.Error
{
    [IocName("LocalError")]
    internal class LocalErrorInit : IRpcInitModular
    {
        public void Init (IIocService ioc)
        {

        }

        public void InitEnd (IIocService ioc, IRpcService service)
        {
        }

        public void Load (RpcInitOption option)
        {
            _ = option.Ioc.RegisterType(typeof(IErrorEvent), typeof(ErrorEvent), "RpcSync");
        }
    }
}
