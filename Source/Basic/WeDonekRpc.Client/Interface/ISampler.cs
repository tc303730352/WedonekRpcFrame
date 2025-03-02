namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 链路采样器
    /// </summary>
    internal interface ISampler
    {
        /// <summary>
        /// 采样器
        /// </summary>
        /// <param name="spanId">链路Id</param>
        /// <returns>是否采样</returns>
        bool Sample(out long spanId);
    }
}