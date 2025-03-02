using WeDonekRpc.Helper.Validate;

namespace Store.Gatewary.Modular.Model
{
    public class StoreLogin
    {
        [NullValidate("rpc.store.loginName.null")]
        [LenValidate("rpc.store.loginName.len", 4, 50)]
        [FormatValidate("rpc.store.loginName.error",  ValidateFormat.数字字母)]
        public string LoginName
        {
            get;
            set;
        }
        [NullValidate("rpc.store.login.pwd.null")]
        [LenValidate("rpc.store.login.name.len", 6, 100)]
        public string LoginPwd
        {
            get;
            set;
        }
    }
}
