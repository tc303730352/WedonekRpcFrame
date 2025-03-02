using WeDonekRpc.Helper.Validate;

namespace Wedonek.Gateway.Modular.Model
{
    /// <summary>
    /// 注册资料
    /// </summary>
    public class UserRegParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [NullValidate("demo.user.name.null")]
        [LenValidate("demo.user.name.len", 2, 50)]
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 用户手机
        /// </summary>
        [NullValidate("demo.user.phone.null")]
        [FormatValidate("demo.user.phone.error", ValidateFormat.手机号)]
        public string UserPhone
        {
            get;
            set;
        }
    }
}
