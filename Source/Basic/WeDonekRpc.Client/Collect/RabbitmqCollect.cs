
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq;
using WeDonekRpc.Client.Rabbitmq.Helper;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;
using System.Collections.Generic;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// Rabbitmq服务集
    /// </summary>
    internal class RabbitmqCollect
    {
        private static RabbitmqConfig _Config = null;
        private static string[] _Queues = null;

        public static void Init()
        {
            _Config = Config.WebConfig.GetRabbitmqConfig();
            if (_Config != null)
            {
                _InitQueue(RpcStateCollect.LocalConfig);
            }
        }

        private static void _InitQueue(LocalRpcConfig source)
        {
            string[] exchanges = QueueHelper.GetExchangeList(source, out string[] broadcasts);
            string[] dead = new string[] {
                    string.Concat ("Rpc_Msg_Dead_", source.RegionId),
                    string.Concat ("Rpc_Sub_Dead_", source.RegionId)
            };
            using (RabbitmqQueue quque = new RabbitmqQueue(_Config))
            {
                using (RabbitmqHelper helper = new RabbitmqHelper(quque))
                {
                    //绑定交换机
                    helper.BindExchange(exchanges);
                    helper.BindExchange(broadcasts);

                    //绑定死信交换机
                    helper.BindExchange(dead, "fanout");

                    //为节点创建队列接收广播
                    string name = string.Concat("Rpc_", source.ServerId);
                    _Queues = new string[]
                    {
                        string.Join("_", "Rpc", source.SystemType, source.RpcMerId),
                        string.Join("_", "Rpc",source.SysGroup, source.RpcMerId),
                        name
                    };
                    //绑定队列
                    helper.QueueDeclare(_Queues, new Dictionary<string, object>
                    {
                            { "x-dead-letter-exchange",dead[0]}
                    });

                    helper.QueueBind(exchanges, new QueueSubInfo[]
                    {
                            new QueueSubInfo
                            {
                                    //已节点类别创建主队列接收广播
                                    Queue=string.Join("_", "Rpc", source.SystemType,source.RpcMerId),
                                    RouteKey=source.SystemType
                            },
                                new QueueSubInfo
                            {
                                    //已节点组创建主队列接收广播
                                    Queue=string.Join("_", "Rpc",source.SysGroup,source.RpcMerId),
                                    RouteKey=source.SysGroup
                            },
                            new QueueSubInfo
                            {
                                    Queue= name,
                                    RouteKey=name
                            },
                            new QueueSubInfo
                            {
                                    Queue= name,
                                    RouteKey="ALLNODE"
                            }
                     });
                    helper.QueueBind(broadcasts, new QueueSubInfo[]
                    {
                            new QueueSubInfo
                            {
                                    Queue=name,
                                    RouteKey= source.SystemType
                            },
                            new QueueSubInfo
                            {
                                    Queue=name,
                                    RouteKey= source.SysGroup
                            },
                            new QueueSubInfo
                            {
                                    Queue=name,
                                    RouteKey= "ALLNODE"
                            }
                    });

                    string[] subQueue = QueueHelper.GetSubscribQueue(source);
                    helper.QueueDeclare(subQueue, new Dictionary<string, object>
                    {
                            { "x-dead-letter-exchange",dead[1]}
                    });
                }
            }
        }
        public static IQueue CreateMsgQueue(SubscribeEvent action)
        {
            string[] exchanges = QueueHelper.GetExchangeList(RpcStateCollect.LocalConfig, out string[] broadcasts);
            SubscribBody[] bodys = exchanges.ConvertAll(_Queues, (a, b) => new SubscribBody
            {
                Exchange = a,
                Queue = b
            });
            string name = string.Concat("Rpc_", RpcStateCollect.ServerId);
            bodys = bodys.Add(broadcasts.ConvertAll(a => new SubscribBody
            {
                Exchange = a,
                Queue = name
            }));
            RabbitmqSet config = new RabbitmqSet
            {
                Config = _Config,
                IsAutoAck = _Config.IsAutoAck,
                Subscribs = bodys
            };
            return new QueueRabbitmq(config, action);
        }
        public static IQueue CreateSubQueue(SubscribeEvent action)
        {
            RabbitmqSet config = new RabbitmqSet
            {
                Config = _Config,
                IsAutoAck = _Config.SubIsAutoAck,
                Subscribs = QueueHelper.GetSubscribBody(RpcStateCollect.LocalConfig)
            };
            return new QueueRabbitmq(config, action);
        }
    }
}
