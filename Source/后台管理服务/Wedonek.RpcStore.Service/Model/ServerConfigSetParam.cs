using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        public class ServerConfigSetParam
        {
                /// <summary>
                /// 服务名
                /// </summary>
                [NullValidate("rpc.server.name.null")]
                [LenValidate("rpc.server.name.len", 2, 50)]
                public string ServerName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务Ip
                /// </summary>
                [NullValidate("rpc.server.ip.null")]
                [FormatValidate("rpc.server.ip.error", ValidateFormat.IP)]
                public string ServerIp
                {
                        get;
                        set;
                }
                /// <summary>
                /// 远程Ip
                /// </summary>
                [FormatValidate("rpc.remote.ip.error", ValidateFormat.IP)]
                public string RemoteIp
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务端口
                /// </summary>
                [NumValidate("rpc.port.error", 1, int.MaxValue)]
                public int ServerPort
                {
                        get;
                        set;
                }

                /// <summary>
                /// 服务Mac
                /// </summary>
                [NullValidate("rpc.mac.null")]
                [FormatValidate("rpc.mac.error", ValidateFormat.MAC)]
                public string ServerMac
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点所在区域
                /// </summary>
                [NumValidate("rpc.server.index.error", 1)]
                public int RegionId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 权值
                /// </summary>
                [NumValidate("rpc.server.weight.error", 0, int.MaxValue)]
                public int Weight
                {
                        get;
                        set;
                }
                /// <summary>
                /// 远程配置优先级
                /// </summary>
                [NumValidate("rpc.config.prower.error", 0, short.MaxValue)]
                public short ConfigPrower
                {
                        get;
                        set;
                }
                /// <summary>
                /// 分发配置
                /// </summary>
                public TransmitConfig[] TransmitConfig
                {
                        get;
                        set;
                }

                /// <summary>
                /// 链接公钥
                /// </summary>
                [NullValidate("rpc.public.key.null")]
                [LenValidate("rpc.public.key.len", 6, 50)]
                public string PublicKey
                {
                        get;
                        set;
                }
        }
}
