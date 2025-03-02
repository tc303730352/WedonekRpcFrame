using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerConfig.Model
{
    /// <summary>
    /// 服务配置
    /// </summary>
    public class ServerConfigSet
    {
        /// <summary>
        /// 自定义编号
        /// </summary>
        [FormatValidate("rpc.store.server.code.error", ValidateFormat.数字字母)]
        [LenValidate("rpc.store.server.code.len", 0, 50)]
        public string ServerCode
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        [NullValidate("rpc.store.server.name.null")]
        [LenValidate("rpc.store.server.name.len", 2, 50)]
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Ip
        /// </summary>
        [NullValidate("rpc.store.server.ip.null")]
        [FormatValidate("rpc.store.server.ip.error", new ValidateFormat[]
        {
            ValidateFormat.IP,
            ValidateFormat.IP6
        })]
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 远程Ip
        /// </summary>
        [FormatValidate("rpc.store.remote.ip.error", new ValidateFormat[]
        {
            ValidateFormat.IP,
            ValidateFormat.IP6
        })]
        public string RemoteIp
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端口
        /// </summary>
        [NumValidate("rpc.store.port.error", 1, int.MaxValue)]
        public int ServerPort
        {
            get;
            set;
        }

        /// <summary>
        /// 远程端口
        /// </summary>
        [NumValidate("rpc.store.remote.port.error", 1, int.MaxValue)]
        public int RemotePort
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Mac
        /// </summary>
        [NullValidate("rpc.store.mac.null")]
        [FormatValidate("rpc.store.mac.error", ValidateFormat.MAC)]
        public string ServerMac
        {
            get;
            set;
        }


        /// <summary>
        /// 远程配置优先级
        /// </summary>
        [NumValidate("rpc.store.config.prower.error", 0, short.MaxValue)]
        public short ConfigPrower
        {
            get;
            set;
        }


        /// <summary>
        /// 链接公钥
        /// </summary>
        [NullValidate("rpc.store.server.public.key.null")]
        [LenValidate("rpc.store.server.public.key.len", 6, 50)]
        public string PublicKey
        {
            get;
            set;
        }

        /// <summary>
        /// 节点熔断恢复后临时限流量
        /// </summary>
        [NumValidate("rpc.store.server.recovery.limit.error", 0, int.MaxValue)]
        public int RecoveryLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 节点熔断恢复后临时限流时长
        /// </summary>
        [NumValidate("rpc.store.server.recovery.time.error", 0, int.MaxValue)]
        public int RecoveryTime
        {
            get;
            set;
        }
        /// <summary>
        /// 持有的服务集群错误
        /// </summary>
        [NumValidate("rpc.store.hold.mer.id.error", 1)]
        public long HoldRpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 当前版本号
        /// </summary>
        [NumValidate("rpc.store.ver.num.error", 0, int.MaxValue)]
        public int VerNum
        {
            get;
            set;
        }
    }
}
