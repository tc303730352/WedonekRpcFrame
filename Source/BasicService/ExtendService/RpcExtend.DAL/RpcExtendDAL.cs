using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL
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
            _ = option.Ioc.RegisterType(typeof(IRpcBasicRepository<>), typeof(Repository.RpcBasicRepository<>));
            SqlSugarService.Init(new SqlSugarUnity(option.Ioc));
        }
    }
}
