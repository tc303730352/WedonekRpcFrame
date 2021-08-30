
using ApiGateway.Attr;

using RpcHelper.Validate;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Gateway.Model;

namespace Wedonek.RpcStore.Gateway.Controller
{
        /// <summary>
        /// 管理员
        /// </summary>
        internal class AdminController : HttpApiGateway.ApiController
        {
                private readonly IAdminService _Admin = null;
                public AdminController(IAdminService admin)
                {
                        this._Admin = admin;
                }
                /// <summary>
                /// 管理员登陆
                /// </summary>
                /// <param name="login"></param>
                /// <returns></returns>
                [ApiPrower(false)]
                public string Login(AdminLogin login)
                {
                        return this._Admin.AdminLogin(login);
                }

                /// <summary>
                /// 设置密码
                /// </summary>
                /// <param name="pwd"></param>
                public void SetPwd(
                        [NullValidate("rpc.login.pwd.null")]
                [LenValidate("rpc.login.pwd.len", 6, 100)]string pwd)
                {
                        this._Admin.SetAdminPwd(this.UserState, pwd);
                }

                /// <summary>
                /// 注册管理员
                /// </summary>
                [ApiPrower(false)]
                public void RegAdmin(AdminLogin reg)
                {
                        this._Admin.RegAdmin(reg, this.Request.ClientIp);
                }
        }
}
