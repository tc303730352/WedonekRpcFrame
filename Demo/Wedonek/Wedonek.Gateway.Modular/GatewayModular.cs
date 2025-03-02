using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Config;

namespace Wedonek.Gateway.Modular
{
    /// <summary>
    /// 网关模块
    /// </summary>
    public class GatewayModular : BasicApiModular
    {
        public GatewayModular () : base("demo")
        {

        }
        protected override void Load ( IHttpGatewayOption option, IModularConfig config )
        {
            option.AddFileDir(new FileDirConfig
            {
                DirPath = "WebDemo",
                Extension = new string[] { "js", "css", "html" },
                VirtualPath = "/demo"
            });
        }
        /// <summary>
        /// 模块初始化
        /// </summary>
        protected override void Init ()
        {
            this.Config.ApiRouteFormat = "/demo/{controller}/{name}";
        }

    }
}
