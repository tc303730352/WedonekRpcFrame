using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    public class TransmitDatum
    {
        /// <summary>
        /// 服务节点编号
        /// </summary>
        [NullValidate("rpc.store.server.code.null")]
        [LenValidate("rpc.store.server.code.len", 2, 50)]
        public string ServerCode { get; set; }

        /// <summary>
        /// 负载配置
        /// </summary>
        [NullValidate("rpc.store.transmit.config.null")]
        public TransmitConfig[] TransmitConfig
        {
            get;
            set;
        }
    }
}
