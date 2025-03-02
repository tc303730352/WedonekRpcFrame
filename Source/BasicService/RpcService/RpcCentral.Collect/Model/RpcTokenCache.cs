namespace RpcCentral.Collect.Model
{
    [Serializable]
    public class RpcTokenCache
    {
        public string TokenId
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// 有效截止时间
        /// </summary>
        public int EffectiveTime
        {
            get;
            set;
        }
        /// <summary>
        /// 应用版本号
        /// </summary>
        public int AppVerNum { get; set; }

        /// <summary>
        /// 当前连接的服务器ID
        /// </summary>
        public long ConServerId { get; set; }

      
    }
}
