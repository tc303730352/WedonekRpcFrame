namespace RpcStore.RemoteModel.Mer.Model
{
    public class RpcMerSet
    {
        /// <summary>
        /// 系统名
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string AppSecret
        {
            get;
            set;
        }
        /// <summary>
        /// 允许访问的服务器IP
        /// </summary>
        public string[] AllowServerIp
        {
            get;
            set;
        }

    }
}
