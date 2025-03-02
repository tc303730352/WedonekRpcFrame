using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 后台管理
    /// </summary>
    internal class StoreApi : ApiController
    {
        private readonly IStoreService _Service;


        public StoreApi (IStoreService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [ApiPrower(false)]
        public LoginResult Login (StoreLogin login)
        {
            return this._Service.Login(login);
        }
        /// <summary>
        /// 检查登陆状态
        /// </summary>
        public void CheckLogin ()
        {

        }
        /// <summary>
        /// 退出登陆
        /// </summary>
        public void LoginOut ()
        {
            this._Service.LoginOut(base.AccreditId);
        }
    }
}
