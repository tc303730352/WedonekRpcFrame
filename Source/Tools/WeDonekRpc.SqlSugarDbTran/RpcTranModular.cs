using System.Reflection;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.SqlSugar;
using WeDonekRpc.SqlSugarDbTran.Attr;

namespace WeDonekRpc.SqlSugarDbTran
{
    public class RpcTranModular : IRpcInitModular
    {
        private static readonly Type _AttrType = typeof(RpcDbTransaction);
        public void Init (IIocService ioc)
        {
        }

        public void InitEnd (IIocService ioc, IRpcService service)
        {
        }

        public void Load (RpcInitOption option)
        {
            option.Tran.RegTwoPcTran<RpcDbTranService>("SugarDBTran");
            option.LoadEnding += this.Option_LoadEnding;
            SqlSugarService.Init(new SqlSugarUnity(option.Ioc));
        }

        private void Option_LoadEnding (RpcInitOption option)
        {
            IRoute[] routes = option.Route.GetAllRoutes();
            if (routes.IsNull())
            {
                return;
            }
            routes.ForEach(a =>
            {
                if (a.Source != null)
                {
                    Attribute mode = a.Source.GetCustomAttribute(_AttrType);
                    if (mode != null)
                    {
                        option.Tran.RegTwoPcTran<RpcDbTranService>(a.RouteName);
                    }
                }
            });
        }
    }
}
