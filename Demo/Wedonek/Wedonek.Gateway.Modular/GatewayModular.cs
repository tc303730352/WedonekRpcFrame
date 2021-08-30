namespace Wedonek.Gateway.Modular
{
        /// <summary>
        /// 网关模块
        /// </summary>
        public class GatewayModular : HttpApiGateway.BasicApiModular
        {
                public GatewayModular() : base("demo")
                {

                }
                /// <summary>
                /// 模块初始化
                /// </summary>
                protected override void Init()
                {
                        this.Config.RegUserState<UserLoginState>();
                        this.Config.ApiRouteFormat = "/api/{controller}/{name}";
                }

        }
}
