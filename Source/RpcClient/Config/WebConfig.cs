
using RpcCacheClient.Config;

using RpcClient.Config.Model;
using RpcClient.Interface;
using RpcClient.Queue.Model;
using RpcClient.Track.Config;

using RpcHelper;
using RpcHelper.Config;

namespace RpcClient.Config
{
        internal class WebConfig
        {
                public static readonly string ApiVerNum = "1.0.2";

                static WebConfig()
                {
                        RpcClient.Config.AddRefreshEvent(_InitConfig);
                }

                /// <summary>
                /// 系统配置
                /// </summary>
                public static RpcSysConfig RpcConfig
                {
                        get;
                        private set;
                }


                internal static TrackConfig GetTrackConfig()
                {
                        TrackConfig config = RpcClient.Config.GetConfigVal("track", new TrackConfig());
                        if (config.ServiceName.IsNull())
                        {
                                config.ServiceName = Collect.RpcStateCollect.ServerConfig.Name;
                        }
                        return config;
                }

                private static void _InitConfig(IConfigServer config,string name)
                {
                        if (name.StartsWith("rpc_config") || name == string.Empty)
                        {
                                RpcConfig = config.GetConfigVal<RpcSysConfig>("rpc_config", new RpcSysConfig());
                        }
                }

                #region 基础配置
                /// <summary>
                /// 本机地址
                /// </summary>
                public static string LocalMac => LocalConfig.Local["rpc:LocalMac"];

                /// <summary>
                /// 中控服务链接地址
                /// </summary>
                public static string[] RpcServer => LocalConfig.Local["rpc:RpcServer", typeof(string[])];
                /// <summary>
                /// 集群AppId
                /// </summary>
                public static string AppId => LocalConfig.Local["rpc:RpcAppId"];
                /// <summary>
                /// 集群链接密钥
                /// </summary>
                public static string AppSecret => LocalConfig.Local["rpc:RpcAppSecret"];
                /// <summary>
                /// 服务节点类型
                /// </summary>
                public static string RpcSystemType => LocalConfig.Local["rpc:RpcSystemType"];

                /// <summary>
                /// 服务节点索引
                /// </summary>
                public static int RpcServerIndex => LocalConfig.Local.GetValue("rpc:RpcIndex", 0);

                /// <summary>
                /// 是否启用错误管理
                /// </summary>
                public static bool IsEnableError => LocalConfig.Local.GetValue("rpc:IsEnableError", true);

                #endregion


                public static CacheConfig GetCacheConfig()
                {
                        CacheConfig config = new CacheConfig
                        {
                                SysKey = RpcSystemType,
                                Memcached = RpcClient.Config.GetConfigVal<MemcachedConfig>("memcached"),
                                Redis = RpcClient.Config.GetConfigVal<RedisConfig>("redis"),
                        };
                        return config;
                }
                public static QueueConfig GetQueueConfig()
                {
                        QueueConfig config = RpcClient.Config.GetConfigVal<QueueConfig>("queue");
                        config.Expiration = RpcConfig.ExpireTime * 1000;
                        config.ServerId = RpcClient.CurrentSource.SourceServerId;
                        config.SysGroupVal = RpcClient.CurrentSource.GroupTypeVal;
                        config.RegionId = RpcClient.CurrentSource.RegionId;
                        config.RpcMerId = RpcClient.CurrentSource.RpcMerId;
                        config.SystemVal = RpcClient.SystemTypeVal;
                        return config;
                }
        }
}
