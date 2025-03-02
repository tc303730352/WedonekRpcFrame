using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerPublic.Model
{
    public class PublicSchemeQuery
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.mer.Id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }

        public string QueryKey { get; set; }


        public SchemeStatus[] Status { get; set; }

        public DateTime? Begin { get; set; }

        public DateTime? End { get; set; }
    }
}
