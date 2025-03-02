namespace WeDonekRpc.ApiGateway.Interface
{
        /// <summary>
        /// 全局限流插件
        /// </summary>
        public interface IWholeLimitPlugIn:IPlugIn
        {
                bool IsLimit();
        }
}