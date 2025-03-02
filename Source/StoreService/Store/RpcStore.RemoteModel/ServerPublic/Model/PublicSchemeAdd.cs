namespace RpcStore.RemoteModel.ServerPublic.Model
{
    public class PublicSchemeAdd : PublicScheme
    {
        /// <summary>
        /// 所属集群
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
    }
}
