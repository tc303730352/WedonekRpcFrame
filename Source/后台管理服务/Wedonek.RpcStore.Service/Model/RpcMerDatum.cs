using RpcHelper.Validate;
namespace Wedonek.RpcStore.Service.Model
{
        public class RpcMerDatum
        {
                /// <summary>
                /// 系统名
                /// </summary>
                [NullValidate("rpc.system.name.null")]
                [LenValidate("rpc.system.name.len", 2, 50)]
                public string SystemName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 应用APPId
                /// </summary>
                [NullValidate("rpc.appId.null")]
                [LenValidate("rpc.appId.len", 32, 32)]
                public string AppId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 应用秘钥
                /// </summary>
                [NullValidate("rpc.app.secret.null")]
                [LenValidate("rpc.app.secret.len", 32, 32)]
                public string AppSecret
                {
                        get;
                        set;
                }
                /// <summary>
                /// 允许访问的服务器IP
                /// </summary>
                [RegexValidate("rpc.ip.error", @"^([*]|((\d{1,3}[.]){3}\d{1,3})){1}$")]
                public string[] AllowServerIp
                {
                        get;
                        set;
                }
        }
}
