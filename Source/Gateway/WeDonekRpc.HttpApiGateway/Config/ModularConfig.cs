using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Interface;
namespace WeDonekRpc.HttpApiGateway.Config
{
    [IgnoreIoc]
    internal class ModularConfig : IModularConfig
    {
        public ModularConfig ()
        {
            this.ApiRouteFormat = ApiGatewayService.Config.ApiRouteFormat;
            this.IsAccredit = true;
        }
        /// <summary>
        /// 是否登陆认证
        /// </summary>
        public bool IsAccredit
        {
            get;
            set;
        }
        /// <summary>
        ///  Api 接口路径生成格式
        /// </summary>
        public string ApiRouteFormat
        {
            get;
            set;
        }
    }
}
