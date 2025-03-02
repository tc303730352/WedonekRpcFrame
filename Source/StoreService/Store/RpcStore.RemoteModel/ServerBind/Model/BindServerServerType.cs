namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class BindServerServerType
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 服务名
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }
        public string TypeVal { get; set; }
        /// <summary>
        /// 服务数量
        /// </summary>
        public int ServerNum { get; set; }
    }
}
