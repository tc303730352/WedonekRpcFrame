namespace WeDonekRpc.ApiGateway.Interface
{
    internal interface IIpLimit
    {
        /// <summary>
        /// IP地址
        /// </summary>
        string Ip { get; }
        /// <summary>
        /// 是否限制
        /// </summary>
        /// <returns></returns>
        bool IsLimit ();
        /// <summary>
        /// 刷新限制
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        int Refresh (int now);
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="config"></param>
        void Reset (IIpLimitConfig config);
    }
}