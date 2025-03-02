using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Helper
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

        public static string[] GetExchangeList (LocalRpcConfig source, out string[] broadcasts)
        {
            broadcasts = GetBroadcast(source);
            return GetExchanges(source);
        }
        public static string[] GetBroadcast (LocalRpcConfig source)
        {
            return new string[]
            {
                    "RpcBroadcast_0",
                    string.Concat("RpcBroadcast_", source.RegionId),
                    //string.Join("_", "Rpc_Broadcast", "0", config.RegionId),
                    string.Join("_", "Rpc_Broadcast", source.RpcMerId, source.RegionId),
                    string.Join("_","Rpc_Broadcast", source.RpcMerId,"0")
            };
        }
        public static string[] GetExchanges (LocalRpcConfig source)
        {
            return new string[]
               {
                    "RpcExchange_0",
                    string.Concat("RpcExchange_", source.RegionId),
                    //string.Join("_", "Rpc", "0", config.RegionId),
                    string.Join("_", "Rpc", source.RpcMerId, source.RegionId),
                    string.Join("_","Rpc", source.RpcMerId,"0")
               };
        }

        public static ConServer[] GetServer (QueueCon[] cons, int defPort = 5672)
        {
            List<ConServer> point = [];
            cons.ForEach(c =>
            {
                if (c.ServerUri != null)
                {
                    IPAddress[] addrs = Dns.GetHostAddresses(c.ServerUri.Host);
                    if (addrs.IsNull())
                    {
                        return;
                    }
                    int port = c.ServerUri.IsDefaultPort ? defPort : c.ServerUri.Port;
                    addrs.ForEach(a =>
                    {
                        point.Add(new ConServer
                        {
                            ip = a.ToString(),
                            port = port
                        });
                    });
                }
                else
                {
                    point.Add(_GetServer(c.ServerIp, defPort));
                }
            });
            return point.Distinct().ToArray();
        }
        private static ConServer _GetServer (string serverIp, int port)
        {
            if (!serverIp.Contains(':'))
            {
                return new ConServer
                {
                    ip = serverIp,
                    port = port
                };
            }
            else
            {
                string[] t = serverIp.Split(':');
                return new ConServer
                {
                    ip = t[0],
                    port = int.Parse(t[1])
                };
            }
        }
        public static string[] GetSubscribQueue (LocalRpcConfig source)
        {
            return new string[]
            {
                string.Join("_", "Rpc_Sub", source.SystemType, source.RpcMerId),
                string.Concat("Rpc_Sub_", source.ServerId)
            };
        }

        public static SubscribBody[] GetSubscribBody (LocalRpcConfig config)
        {
            string one = string.Concat("Rpc_Sub_", config.ServerId);
            string name = string.Join("_", "Rpc_Sub", config.SystemType, config.RpcMerId);
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
        private static string _GetExchange (IRemoteBroadcast msg)
        {
            if (msg.IsOnly)
            {
                return string.Join("_", "Rpc", msg.RpcMerId.GetValueOrDefault(), msg.RegionId.GetValueOrDefault());
            }
            else
            {
                return string.Join("_", "Rpc_Broadcast", msg.RpcMerId.GetValueOrDefault(), msg.RegionId.GetValueOrDefault());
            }
        }
        private static string _GetExchange (IRemoteBroadcast msg, long rpcMerId)
        {
            if (msg.IsOnly)
            {
                return string.Join("_", "Rpc", rpcMerId, msg.RegionId.GetValueOrDefault());
            }
            else
            {
                return string.Join("_", "Rpc_Broadcast", rpcMerId, msg.RegionId.GetValueOrDefault());
            }
        }
        private static string _GetCrossGroupExchange (IRemoteBroadcast msg)
        {
            if (!msg.IsOnly)
            {
                return string.Concat("RpcBroadcast_", msg.RegionId.GetValueOrDefault());
            }
            return string.Concat("RpcExchange_", msg.RegionId.GetValueOrDefault());
        }
        public static string[] GetRouteKey (IRemoteBroadcast msg, long[] serverId, out string exchange)
        {
            exchange = string.Concat("RpcExchange_", msg.RegionId.GetValueOrDefault());
            if (msg.BroadcastType == BroadcastType.订阅)
            {
                return new string[]
                {
                    msg.SysDictate
                };
            }
            return serverId.ConvertAll(a => string.Concat("Rpc_", a));
        }
        public static string[] GetRouteKey (IRemoteBroadcast msg, out string exchange)
        {
            if (!msg.ServerId.IsNull())
            {
                return GetRouteKey(msg, msg.ServerId, out exchange);
            }
            return GetRouteKey(msg, msg.RpcMerId.GetValueOrDefault(), msg.TypeVal, out exchange);
        }
        internal static string[] GetRouteKey (IRemoteBroadcast msg, long rpcMerId, string[] typeVal, out string exchange)
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
        private static readonly Type _byteType = typeof(byte[]);


        public static object GetValue (byte[] data, Type type)
        {
            if (type == _byteType)
            {
                return data;
            }
            if (type == PublicDataDic.StrType)
            {
                return Encoding.UTF8.GetString(data);
            }
            else if (type == PublicDataDic.IntType)
            {
                return BitConverter.ToInt32(data);
            }
            else if (type == PublicDataDic.LongType)
            {
                return BitConverter.ToInt64(data);
            }
            else if (type == PublicDataDic.ShortType)
            {
                return BitConverter.ToInt16(data);
            }
            else if (type == PublicDataDic.BoolType)
            {
                return BitConverter.ToBoolean(data);
            }
            else if (type == PublicDataDic.DateTimeType)
            {
                return BitConverter.ToInt64(data).ToDateTime();
            }
            else if (type == PublicDataDic.UriType)
            {
                string str = Encoding.UTF8.GetString(data);
                return new Uri(str);
            }
            else if (type.IsClass)
            {
                string str = Encoding.UTF8.GetString(data);
                return str.Json(type);
            }
            else if (type.IsValueType)
            {
                TypeCode code = Type.GetTypeCode(type);
                switch (code)
                {
                    case TypeCode.Byte:
                        return data[0];
                    case TypeCode.SByte:
                        return data[0];
                    case TypeCode.UInt16:
                        return BitConverter.ToUInt16(data);
                    case TypeCode.Char:
                        return BitConverter.ToChar(data);
                    case TypeCode.UInt32:
                        return BitConverter.ToUInt32(data);
                    case TypeCode.Single:
                        return BitConverter.ToSingle(data);
                    case TypeCode.Double:
                        return BitConverter.ToDouble(data);
                    case TypeCode.UInt64:
                        return BitConverter.ToUInt64(data);
                    case TypeCode.Decimal:
                        string str = Encoding.UTF8.GetString(data);
                        return decimal.Parse(str);
                }
            }
            return null;
        }
    }
}
