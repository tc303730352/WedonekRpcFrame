using System.Net;

using RpcClient.Queue.Model;
using RpcClient.Rabbitmq.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Helper
{

        internal class QueueHelper
        {
                /// <summary>
                /// 主交换机
                /// </summary>
                public static readonly string MainExchange = "RpcExchange_0";
                /// <summary>
                /// 主广播交换机
                /// </summary>
                public static readonly string MainBroadcastExchange = "RpcBroadcast_0";

                public static string[] GetExchangeList(QueueConfig config, out string[] broadcasts)
                {
                        broadcasts = GetBroadcast(config);
                        return GetExchanges(config);
                }
                public static string[] GetBroadcast(QueueConfig config)
                {
                        return new string[]
                        {
                                "RpcBroadcast_0",
                                string.Concat("RpcBroadcast_", config.RegionId),
                                //string.Join("_", "Rpc_Broadcast", "0", config.RegionId),
                                string.Join("_", "Rpc_Broadcast", config.RpcMerId, config.RegionId),
                                string.Join("_","Rpc_Broadcast", config.RpcMerId,"0")
                        };
                }
                public static string[] GetExchanges(QueueConfig config)
                {
                        return new string[]
                           {
                                "RpcExchange_0",
                                string.Concat("RpcExchange_", config.RegionId),
                                //string.Join("_", "Rpc", "0", config.RegionId),
                                 string.Join("_", "Rpc", config.RpcMerId, config.RegionId),
                                 string.Join("_","Rpc", config.RpcMerId,"0")
                           };
                }

                public static IPEndPoint GetServer(string serverIp, int port)
                {
                        if (serverIp.IndexOf(":") == -1)
                        {
                                return new IPEndPoint(IPAddress.Parse(serverIp), port);
                        }
                        else
                        {
                                string[] t = serverIp.Split(':');
                                return new IPEndPoint(IPAddress.Parse(t[0]), int.Parse(t[1]));
                        }
                }
                public static string[] GetSubscribQueue(QueueConfig config)
                {
                        return new string[]
                        {
                               string.Join("_", "Rpc_Sub", config.SystemVal, config.RpcMerId),
                              string.Concat("Rpc_Sub_", config.ServerId)
                        };
                }
                public static SubscribBody[] GetSubscribBody(QueueConfig config)
                {
                        string one = string.Concat("Rpc_Sub_", config.ServerId);
                        string name = string.Join("_", "Rpc_Sub", config.SystemVal, config.RpcMerId);
                        return new SubscribBody[]
                        {
                                new SubscribBody
                                {
                                        Exchange= string.Join("_", "Rpc", config.RpcMerId, config.RegionId),
                                        Queue=name
                                },
                                new SubscribBody
                                {
                                        Exchange= string.Join("_", "Rpc", config.RpcMerId, "0"),
                                        Queue=name
                                },
                                 new SubscribBody
                                {
                                        Exchange= string.Join("_", "Rpc_Broadcast", config.RpcMerId, config.RegionId),
                                        Queue=one
                                },
                                new SubscribBody
                                {
                                        Exchange= string.Join("_", "Rpc_Broadcast", config.RpcMerId, "0"),
                                        Queue=one
                                }
                        };
                }
                private static string _GetExchange(IRemoteBroadcast msg)
                {
                        if (msg.IsOnly)
                        {
                                return string.Join("_", "Rpc", msg.RpcMerId, msg.RegionId);
                        }
                        else
                        {
                                return string.Join("_", "Rpc_Broadcast", msg.RpcMerId, msg.RegionId);
                        }
                }
                private static string _GetExchange(IRemoteBroadcast msg, long rpcMerId)
                {
                        if (msg.IsOnly)
                        {
                                return string.Join("_", "Rpc", rpcMerId, msg.RegionId);
                        }
                        else
                        {
                                return string.Join("_", "Rpc_Broadcast", rpcMerId, msg.RegionId);
                        }
                }
                private static string _GetCrossGroupExchange(IRemoteBroadcast msg)
                {
                        if (!msg.IsOnly)
                        {
                                return string.Concat("RpcBroadcast_", msg.RegionId);
                        }
                        return string.Concat("RpcExchange_", msg.RegionId);
                }
                public static string[] GetRouteKey(IRemoteBroadcast msg, long[] serverId, out string exchange)
                {
                        exchange = string.Concat("RpcExchange_", msg.RegionId);
                        if (msg.BroadcastType == BroadcastType.订阅)
                        {
                                return new string[]
                                {
                                      msg.SysDictate
                                };
                        }
                        return serverId.ConvertAll(a => string.Concat("Rpc_", a));
                }
                public static string[] GetRouteKey(IRemoteBroadcast msg, out string exchange)
                {
                        if (!msg.ServerId.IsNull())
                        {
                                return GetRouteKey(msg, msg.ServerId, out exchange);
                        }
                        return GetRouteKey(msg, msg.RpcMerId, msg.TypeVal, out exchange);
                }
                internal static string[] GetRouteKey(IRemoteBroadcast msg, long rpcMerId, string[] typeVal, out string exchange)
                {
                        if (msg.BroadcastType == BroadcastType.订阅)
                        {
                                exchange = _GetExchange(msg, rpcMerId);
                                return new string[]
                                {
                                        msg.SysDictate
                                };
                        }
                        else
                        {
                                if (msg.IsCrossGroup)
                                {
                                        exchange = _GetCrossGroupExchange(msg);
                                }
                                else
                                {
                                        exchange = _GetExchange(msg);
                                }
                                if (typeVal.IsNull())
                                {
                                        return new string[] { "ALLNODE" };
                                }
                                return typeVal;
                        }
                }
        }
}
