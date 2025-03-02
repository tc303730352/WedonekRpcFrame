using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Config;

namespace Store.Gatewary.Modular
{
    public class StoreApiModular : BasicApiModular
    {
        /// <summary>
        /// 后台API接口服务
        /// </summary>
        public StoreApiModular () : base("Store_Modular", "后台服务")
        {
        }
        protected override void Init ()
        {
            this.Config.ApiRouteFormat = "/api/{controller}/{name}";
        }
        protected override void Load (IHttpGatewayOption option, IModularConfig config)
        {
            //设置默认生命周期的事件方法修改即将注册的关系结构
            option.IocBuffer.SetDefLifetimeType((body) =>
            {
                if (body.To.FullName.StartsWith("Store.Gatewary.Modular.Interface."))
                {
                    body.SetLifetimeType(ClassLifetimeType.SingleInstance);
                }
            });
            option.AddFileDir(new FileDirConfig
            {
                DirPath = "file",
                Extension = new string[] { "png", "jpg", "gif", ".json" },
                VirtualPath = "/file"
            });
        }
    }
}
