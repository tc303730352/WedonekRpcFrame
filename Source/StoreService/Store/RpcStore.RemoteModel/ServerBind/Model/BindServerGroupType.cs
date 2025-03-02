namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class BindServerGroupType
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
        /// 组别名字
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// 服务类型
        /// </summary>
        public BindServerServerType[] ServerType { get; set; }
    }
}
