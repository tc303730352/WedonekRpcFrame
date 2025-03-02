using AutoTask.Gateway;
using Store.Gatewary.Modular;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.HttpApiDoc;

namespace WeDonekRpc.StoreGateway
{
    internal class Global : BasicGlobal
    {
        public override void Load ( IGatewayOption option )
        {
            //注册业务接口模块
            option.RegModular(new StoreApiModular());
            option.RegModular(new AutoTaskApiModular());
            //注册在线文档模块
            option.RegDoc(new ApiDocModular());
        }
    }
}
