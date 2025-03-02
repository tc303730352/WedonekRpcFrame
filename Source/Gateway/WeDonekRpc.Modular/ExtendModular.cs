using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Modular.SysEvent;

namespace WeDonekRpc.Modular
{
    public class ExtendModular : IRpcInitModular
    {
        public void Init ( IIocService ioc )
        {
            IRpcDirectShieldPlugIn shieId = ioc.Resolve<IRpcDirectShieldPlugIn>();
            shieId.Init();
        }
        public void Load ( RpcInitOption option )
        {
            option.LoadModular<CacheModular.CacheModular>();
        }
        public void InitEnd ( IIocService ioc, IRpcService service )
        {
            RpcEventService.Init(service);
        }
    }
}
