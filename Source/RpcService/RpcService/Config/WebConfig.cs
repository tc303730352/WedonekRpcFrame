using RpcCacheClient.Config;
using RpcCacheClient.Interface;

using RpcService.Controller;
using RpcService.Model;

using RpcHelper;
using RpcHelper.Config;
namespace RpcService.Config
{
        internal class WebConfig
        {
                private static readonly RpcServerSysConfig _SysConfig = null;
                static WebConfig()
                {
                        CacheType = LocalConfig.Local.GetValue("rpc:CacheType", CacheType.Redis);
                        _SysConfig = LocalConfig.Local.GetValue<RpcServerSysConfig>("sys");
                }
                public static CacheConfig GetCache()
                {
                        CacheConfig cache = new CacheConfig
                        {
                                SysKey = string.Concat("Rpc_", WebConfig.ServerIndex),
                                Memcached = LocalConfig.Local.GetValue<MemcachedConfig>("memcached"),
                                Redis = LocalConfig.Local.GetValue<RedisConfig>("redis"),
                        };
                        return cache;
                }
                public static int ServerIndex
                {
                        get;
                        private set;
                } = LocalConfig.Local.GetValue("rpc:ServerIndex", 0);

                public static int CacheVerNum
                {
                        get;
                        private set;
                }

                public static CacheType CacheType { get; } = CacheType.Redis;

                public static void SendOnline(RpcServerController server)
                {
                        if (!_SysConfig.ServerStateNotice || _SysConfig.EmailList.Length == 0)
                        {
                                return;
                        }
                        string content = string.Format("服务器( {0} ) 恢复!</br>服务器IP:{1}:{2}</br>服务器ID：{3} </br>服务器Type: {4}", server.ServerName, server.RemoteIp, server.ServerPort, server.ServerId, server.TypeVal);
                        EmailModel model = new EmailModel
                        {
                                DisplayName = _SysConfig.DisplayName,
                                Reciver = _SysConfig.EmailList,
                                Content = content,
                                Title = string.Format("RPC服务端恢复-{0}", server.ServerName),
                                EmailAccount = _SysConfig.EmailAccount,
                                EmailPwd = _SysConfig.EmailPwd
                        };
                        RpcHelper.EmailTools.SendEmail(model);
                }
                public static void SendOffline(RpcServerController server)
                {
                        if (!_SysConfig.ServerStateNotice || _SysConfig.EmailList.Length == 0)
                        {
                                return;
                        }
                        string content = string.Format("服务器( {0} ) 离线!</br>服务器IP:{1}:{2}</br>服务器ID：{3} </br>服务器Type:{4}", server.ServerName, server.RemoteIp, server.ServerPort, server.ServerId, server.TypeVal);
                        EmailModel model = new EmailModel
                        {
                                DisplayName = _SysConfig.DisplayName,
                                Reciver = _SysConfig.EmailList,
                                Content = content,
                                Title = string.Format("RPC服务端离线-{0}", server.ServerName),
                                EmailAccount = _SysConfig.EmailAccount,
                                EmailPwd = _SysConfig.EmailPwd
                        };
                        EmailTools.SendEmail(model);
                }

        }
}
