namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// 响应
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// 初始化响应
        /// </summary>
        /// <param name="service">网关模块</param>
        void InitResponse(IApiService service);
        /// <summary>
        /// 检查响应数据
        /// </summary>
        /// <param name="service">网关模块</param>
        /// <returns>是否验证通过</returns>
        bool Verification(IApiService service);
        /// <summary>
        /// 写入流
        /// </summary>
        /// <param name="service">网关模块</param>
        void WriteStream(IApiService service);
    }
}
