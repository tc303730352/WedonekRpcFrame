using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.SqlSugar;

namespace AutoTask.DAL
{
    internal class AutoTaskDAL : IRpcInitModular
    {

        public void Init (IIocService ioc)
        {

        }

        public void InitEnd (IIocService ioc, IRpcService service)
        {
        }

        public void Load (RpcInitOption option)
        {
            SqlSugarService.Init(new SqlSugarUnity(option.Ioc));
        }
    }
}
