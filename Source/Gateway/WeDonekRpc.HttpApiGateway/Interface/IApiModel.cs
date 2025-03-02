using WeDonekRpc.ApiGateway.Model;
using System;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// API接口配置
    /// </summary>
    public interface IApiModel
    {
        /// <summary>
        /// API事件处理
        /// </summary>
        Type ApiEventType { get; set; }
        /// <summary>
        /// API地址
        /// </summary>
        string ApiUri { get; set; }
        /// <summary>
        /// 是否检查授权
        /// </summary>
        bool IsAccredit { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnable { get; set; }
        /// <summary>
        /// API地址格式是否为正则表达式
        /// </summary>
        bool IsRegex { get; set; }
        /// <summary>
        /// 访问权限
        /// </summary>
        string[] Prower { get; set; }
        /// <summary>
        /// 处理上传的配置类
        /// </summary>
        Type UpConfig { get; set; }
        /// <summary>
        /// 上传配置
        /// </summary>
        ApiUpSet UpSet { get; set; }
    }
}