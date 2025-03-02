namespace RpcStore.RemoteModel.ServerGroup.Model
{
    /// <summary>
    /// 服务组
    /// </summary>
    public class ServerGroupDatum
    {
        /// <summary>
        /// 组ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务组类别值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 组别名字
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }
    }
}
