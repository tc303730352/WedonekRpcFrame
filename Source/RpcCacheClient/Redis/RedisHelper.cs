using System;
using System.Collections.Generic;

using RpcCacheClient.Config;
using RpcCacheClient.Helper;

using StackExchange.Redis;

using RpcHelper;
namespace RpcCacheClient.Redis
{
        internal class RedisHelper
        {
                private static readonly List<ConnectionMultiplexer> _Clients = new List<ConnectionMultiplexer>();
                private static ConnectionMultiplexer _RedisClient = null;
                private static ConnectionMultiplexer _PublicClient = null;
                private static RedisConfig _Config = null;

                public static bool InitCache(RedisConfig config)
                {
                        if (config.ServerIp.IsNull())
                        {
                                return false;
                        }
                        try
                        {
                                _Config = config;
                                ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
                                _PublicClient = _GetRedisClient(CommandMap.Default, "RedisPublic", true);
                                _RedisClient = _GetRedisClient(CommandMap.Twemproxy, "RedisTwemproxy", false);
                                return true;
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(e, "Redis初始化失败!", "Redis")
                                {
                                        LogContent = config.ToJson()
                                }.Save();
                                return false;
                        }
                }
                private static ConnectionMultiplexer _GetRedisClient(CommandMap map, string name, bool useHight)
                {
                        ConfigurationOptions config = new ConfigurationOptions
                        {
                                AbortOnConnectFail = true,
                                DefaultDatabase = _Config.DefaultDatabase,
                                ClientName = RpcCacheService.FormatKey(name),
                                AllowAdmin = _Config.AllowAdmin,
                                CommandMap = map,
                                PreserveAsyncOrder = false,
                                KeepAlive = _Config.KeepAlive,
                                ConnectRetry = _Config.ConnectRetry,
                                SyncTimeout = _Config.SyncTimeout,
                                ConnectTimeout = _Config.ConnectTimeout,
                                ResponseTimeout = _Config.ResponseTimeout,
                                User = _Config.UserName,
                                Password = _Config.Pwd,
                                HighPrioritySocketThreads = _Config.HighPrioritySocketThreads,
                                AsyncTimeout = _Config.AsyncTimeout,
                                CheckCertificateRevocation = _Config.CheckCertificateRevocation,
                                ServiceName = _Config.ServiceName
                        };
                        config.CertificateValidation += _Config_CertificateValidation;
                        config.SocketManager = new SocketManager(name, useHight);
                        _Config.ServerIp.ForEach(a =>
                        {
                                config.EndPoints.Add(CacheHelper.GetServer(a, 6379));
                        });
                        ConnectionMultiplexer client = ConnectionMultiplexer.Connect(config);
                        client.ConnectionFailed += _Client_ConnectionFailed;
                        client.ErrorMessage += _Client_ErrorMessage;
                        client.ConnectionRestored += _Client_ConnectionRestored;
                        client.InternalError += _Client_InternalError;
                        _Clients.Add(client);
                        return client;
                }
                /// <summary>
                /// 每当发生内部错误时引发
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private static void _Client_InternalError(object sender, InternalErrorEventArgs e)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.ERROR))
                        {
                                new ErrorLog(e.Exception, "发生内部错误！", "Redis")
                                {
                                        { "ConnectionType",e.ConnectionType},
                                        { "Origin",e.Origin},
                                        { "EndPoint",e.EndPoint}
                                }.Save();
                        }
                }
                /// <summary>
                /// 在建立物理连接时引发
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private static void _Client_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.ERROR))
                        {
                                new ErrorLog(e.Exception, "Redis连接错误!", "Redis")
                                {
                                        { "ConnectionType",e.ConnectionType},
                                        { "FailureType",e.FailureType},
                                        { "EndPoint",e.EndPoint}
                                }.Save();
                        }
                }
                /// <summary>
                /// 服务器回复错误消息
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private static void _Client_ErrorMessage(object sender, RedisErrorEventArgs e)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.WARN))
                        {
                                new WarnLog("redis.msg.error", "Redis回复错误消息!", "Redis")
                                {
                                        { "Message",e.Message},
                                        { "EndPoint",e.EndPoint}
                                }.Save();
                        }
                }
                /// <summary>
                /// 在物理连接失败时引发
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private static void _Client_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.WARN))
                        {
                                new WarnLog("redis.connection.fail", "Redis链接失败！", "Redis")
                                {
                                        { "ConnectionType",e.ConnectionType},
                                        { "FailureType",e.FailureType},
                                        { "EndPoint",e.EndPoint}
                                }.Save();
                        }
                }

                private static bool _Config_CertificateValidation(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                        return true;
                }

                public static IDatabase GetNewClient(string name)
                {
                        ConnectionMultiplexer client = _GetRedisClient(CommandMap.Default, name, false);
                        return client.GetDatabase();
                }
                public static ISubscriber GetPublic()
                {
                        return _PublicClient.GetSubscriber();
                }
                public static ISubscriber GetSubscriber(string name)
                {
                        ConnectionMultiplexer client = _GetRedisClient(CommandMap.Default, name, false);
                        return client.GetSubscriber();
                }
                public static IDatabase GetPublicClient(int db)
                {
                        return _PublicClient.GetDatabase(db);
                }
                public static IDatabase GetClient(int db)
                {
                        return _RedisClient.GetDatabase(db);
                }

                public static void Close()
                {
                        _PublicClient?.Dispose();
                        _RedisClient?.Dispose();
                        _Clients.ForEach(a => a.Dispose());
                }
        }
}
