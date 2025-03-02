using System;
using System.Net;
using System.Text;
using WeDonekRpc.HttpService.Config;

namespace WeDonekRpc.HttpService.Interface
{
    public interface IHttpConfig
    {
        /// <summary>
        /// 跨越配置
        /// </summary>
        ICrossConfig Cross { get; }
        /// <summary>
        /// 证书Hash值
        /// </summary>
        string CertHashVal { get; }
        /// <summary>
        /// 文件响应配置
        /// </summary>
        HttpFileConfig File { get; }
        /// <summary>
        /// GZIP压缩配置
        /// </summary>
        GzipConfig Gzip { get; }
            
        /// <summary>
        /// 请求日志配置
        /// </summary>
        LogConfig Log { get; }

        /// <summary>
        /// 上传配置
        /// </summary>
        UpConfig Up { get; }
        /// <summary>
        /// 真实请求地址
        /// </summary>
        Uri RealRequestUri { get; }

        /// <summary>
        /// 响应编码
        /// </summary>
        Encoding ResponseEncoding { get; }

        /// <summary>
        /// 请求编码
        /// </summary>
        Encoding RequestEncoding { get; }

        /// <summary>
        /// 忽略写入异常
        /// </summary>
        bool IgnoreWriteExceptions { get; }

        /// <summary>
        /// 超时配置
        /// </summary>
        TimeOutConfig TimeOut { get; }

        /// <summary>
        /// 领域
        /// </summary>
        string Realm { get; }

        /// <summary>
        ///认证方案
        /// </summary>
        AuthenticationSchemes AuthenticationSchemes { get; }

        /// <summary>
        /// 添加物理文件访问目录
        /// </summary>
        /// <param name="config"></param>
        void AddFileDir (HttpFileConfig config);

    }
}