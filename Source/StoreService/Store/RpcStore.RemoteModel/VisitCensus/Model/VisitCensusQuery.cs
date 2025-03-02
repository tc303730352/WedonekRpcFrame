using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.VisitCensus.Model
{
    public class VisitCensusQuery
    {
        /// <summary>
        /// 服务节点
        /// </summary>
        [NumValidate("rpc.store.server.id.null", 1)]
        public long ServiceId
        {
            get;
            set;
        }
        /// <summary>
        /// 指令集
        /// </summary>
        public string Dictate
        {
            get;
            set;
        }
    }
}
