namespace RpcStore.RemoteModel.ServerType.Model
{
    public class ServerTypeQuery
    {
        /// <summary>
        /// 所属组别
        /// </summary>
        public long? GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 类型名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
