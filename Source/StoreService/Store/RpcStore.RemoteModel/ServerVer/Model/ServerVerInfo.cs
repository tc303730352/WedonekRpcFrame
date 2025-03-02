namespace RpcStore.RemoteModel.ServerVer.Model
{
    public class ServerVerInfo
    {
        /// <summary>
        /// 组ID
        /// </summary>
        public long SystemTypeId
        {
            get;
            set;
        }
        public string TypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 当前版本号
        /// </summary>
        public string CurrentVer
        {
            get;
            set;
        }

        /// <summary>
        /// 最新版本号
        /// </summary>
        public string[] LastVerNum { get; set; }
    }
}
