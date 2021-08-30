namespace RpcModel
{
        /// <summary>
        /// 获取远程服务器
        /// </summary>
        public class GetRemoteServer
        {
                public long ServerId
                {
                        get;
                        set;
                }
                public int RegionId
                {
                        get;
                        set;
                }
                public string AccessToken
                {
                        get;
                        set;
                }
        }
}
