namespace RpcModel.Server
{
        /// <summary>
        /// 获取节点限制
        /// </summary>
        public class GetServerLimit
        {
                /// <summary>
                /// 获取TokenId
                /// </summary>
                public string AccessToken
                {
                        get;
                        set;
                }
                public long ServerId
                {
                        get;
                        set;
                }
        }
}
