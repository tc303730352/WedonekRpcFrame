using System;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IGatewayUpConfig
    {
        /// <summary>
        /// 分块上传超时时间
        /// </summary>
        int BlockUpOverTime { get; }

        /// <summary>
        /// 分块上传大小
        /// </summary>
        int BlockUpSize { get; }

        /// <summary>
        /// 分块上传URI地址
        /// </summary>
        string BlockUpUri { get; }

        /// <summary>
        /// 分块上传状态查询配置
        /// </summary>
        RouteSet BlockUpState {  get; }
        /// <summary>
        /// 分块上传URI地址是否是正则表达式
        /// </summary>
        bool BlockUpUriIsRegex { get; }

        /// <summary>
        /// 启用分包上传
        /// </summary>
        bool EnableBlock { get; }

        /// <summary>
        /// 允许的文件扩展名
        /// </summary>
        string[] FileExtension { get; }

        /// <summary>
        /// 上传文件是否计算MD5
        /// </summary>
        bool IsGenerateMd5 { get; }

        /// <summary>
        /// 限定一次请求上传文件数量（0 无限制）
        /// </summary>
        int LimitFileNum { get; }

        /// <summary>
        /// 单文件上传大小
        /// </summary>
        int SingleFileSize { get; }

        /// <summary>
        /// 上传块数
        /// </summary>
        int UpBlockNum { get; }

        void RefreshEvent ( Action<IGatewayUpConfig> refresh );

        /// <summary>
        /// 上传配置
        /// </summary>
        /// <returns></returns>
        ApiUpSet ToUpSet ();

    }
}