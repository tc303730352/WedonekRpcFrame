namespace RpcStore.RemoteModel.ServerType.Model
{
    public class ServerTypeDatum : ServerType
    {
        /// <summary>
        /// 服务组
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务数量
        /// </summary>
        public int ServerNum {  get; set; }
    }
}
