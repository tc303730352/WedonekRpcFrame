namespace RpcStore.RemoteModel.Mer.Model
{
    public class RpcMer
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 系统名
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 应用APPId
        /// </summary>
        public string AppId
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
        /// 拥有的服务数量
        /// </summary>
        public int ServerNum
        {
            get;
            set;
        }
    }
}
