using RpcHelper.Validate;

namespace Wedonek.RpcStore.Gateway.Model
{
        internal class AdminLogin
        {
                /// <summary>
                /// 登陆名
                /// </summary>
                [NullValidate("rpc.login.name.null")]
                [LenValidate("rpc.login.name.len", 5, 30)]
                public string LoginName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 登陆密码
                /// </summary>
                [NullValidate("rpc.login.pwd.null")]
                [LenValidate("rpc.login.pwd.len", 6, 100)]
                public string Pwd
                {
                        get;
                        set;
                }
        }
}
