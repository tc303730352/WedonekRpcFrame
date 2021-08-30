namespace ApiGateway.Interface
{
        /// <summary>
        /// 客户端标识
        /// </summary>
        public interface IClientIdentity
        {
                /// <summary>
                /// 客户端标识
                /// </summary>
                string IdentityId
                {
                        get;
                }
                /// <summary>
                /// 应用名
                /// </summary>
                string AppName { get; }
        }
}
