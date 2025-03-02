namespace WeDonekRpc.Model.Server
{
    public class ApplyAccessToken
    {
        /// <summary>
        /// 授权AppId
        /// </summary>
        public string AppId
        {
            get;
            set;
        }
        /// <summary>
        /// 授权密钥
        /// </summary>
        public string Secret
        {
            get;
            set;
        }
        /// <summary>
        /// 连接服务器ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
    }
}
