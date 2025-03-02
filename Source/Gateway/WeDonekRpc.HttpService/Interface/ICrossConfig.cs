namespace WeDonekRpc.HttpService.Interface
{
    public interface ICrossConfig
    {
        // <summary>
        /// 是否启用
        /// </summary>
        bool IsEnable { get; }
        /// <summary>
        /// 是否允许跨域
        /// </summary>
        bool AllowCredentials { get; }
        /// <summary>
        /// 允许的请求头
        /// </summary>
        string AllowHead { get; }
        /// <summary>
        /// 允许来源地址
        /// </summary>
        string[] AllowUrlReferrer { get; }
        /// <summary>
        /// 最大缓存时间
        /// </summary>
        string MaxAge { get; }
        /// <summary>
        /// 允许的谓词
        /// </summary>
        string Method { get; }
    }
}