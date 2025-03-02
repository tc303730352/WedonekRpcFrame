using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL
{
    internal class RpcExtendDAL : IRpcInitModular
    {
        public void Init (IIocService ioc)
        {

        }

        public void InitEnd (IIocService ioc, IRpcService service)
        {
        }

        public void Load (RpcInitOption option)
        {
            _ = option.Ioc.RegisterType(typeof(IRpcExtendResource<>), typeof(Repository.RpcExtendResource<>));
            SqlSugarService.Init(new SqlSugarUnity(option.Ioc));
        }
    }
}
