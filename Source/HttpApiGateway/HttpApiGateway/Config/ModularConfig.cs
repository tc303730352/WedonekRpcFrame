
using HttpApiGateway.Interface;

using RpcModular;
using RpcModular.Model;

namespace HttpApiGateway.Config
{
        [RpcClient.Attr.IgnoreIoc]
        internal class ModularConfig : IModularConfig
        {
                private IAccreditService _Accredit;

                public ModularConfig()
                {
                        this._Accredit = RpcClient.RpcClient.Unity.Resolve<IAccreditService>();
                        this.ApiRouteFormat = ApiGatewayService.Config.ApiRouteFormat;
                }
                /// <summary>
                ///  Api 接口路径生成格式
                /// </summary>
                public string ApiRouteFormat
                {
                        get;
                        set;
                }

                public void RegUserState<T>() where T : UserState
                {
                        this._Accredit = this._Accredit.Create<T>();
                }

                public IUserState GetAccredit(string accreditId)
                {
                        return this._Accredit.GetAccredit(accreditId);
                }
        }
}
