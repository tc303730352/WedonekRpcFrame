using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Wedonek.Gateway.Modular.Controller
{
    /// <summary>
    /// 用户接口
    /// </summary>
    internal class UserController : ApiController
    {
        private readonly IUserService _User;

        public UserController (IUserService user)
        {
            this._User = user;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="reg">用户资料</param>
        /// <returns>用户Id</returns>
        [ApiPrower(false)]
        public long Reg (UserRegParam reg)
        {
            return this._User.Reg(reg);
        }

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <returns>用户资料</returns>
        public UserDatum Get ()
        {
            return this._User.GetUser(this.UserState);
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>授权码</returns>
        [ApiPrower(false)]
        public string Login (
                [NullValidate("demo.user.phone.null")]
                [FormatValidate("demo.user.phone.error", ValidateFormat.手机号)]
                string phone)
        {
            return this._User.UserLogin(phone);
        }

    }
}
