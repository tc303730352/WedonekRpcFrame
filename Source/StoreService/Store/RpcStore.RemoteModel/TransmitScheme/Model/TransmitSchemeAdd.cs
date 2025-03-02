using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    public class TransmitSchemeAdd : TransmitSchemeSet
    {

        /// <summary>
        /// 所属集群
        /// </summary>
        [NumValidate("rpc.store.mer.Id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
    }
}
