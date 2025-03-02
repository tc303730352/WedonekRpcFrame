using WeDonekRpc.Modular;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public interface IModularConfig
    {
        /// <summary>
        /// 是否需要登陆认证
        /// </summary>
        bool IsAccredit { get; set; }
        /// <summary>
        ///   Api 接口地址生成格式
        /// </summary>
        string ApiRouteFormat { get; set; }
    }
}