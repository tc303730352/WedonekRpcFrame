namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class ServerBindVer
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
        /// 系统类别
        /// </summary>
        public SystemTypeBindVer[] SystemType
        {
            get;
            set;
        }
    }
}
