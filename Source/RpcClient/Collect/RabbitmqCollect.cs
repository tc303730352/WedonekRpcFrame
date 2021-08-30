using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Queue.Model;
using RpcClient.Rabbitmq;
using RpcClient.Rabbitmq.Helper;
using RpcClient.Rabbitmq.Model;

using RpcHelper;

namespace RpcClient.Collect
{
        internal class RabbitmqCollect
        {
                private static QueueConfig _Config = null;

                private static string[] _Queues = null;

                public static QueueConfig Config => _Config;

                public static void Init(QueueConfig config)
                {
                        _Config = config;
                        _InitQueue();
                }

                private static void _InitQueue()
                {
                        string[] exchanges = QueueHelper.GetExchangeList(_Config, out string[] broadcasts);
                        using (RabbitmqQueue quque = new RabbitmqQueue(_Config))
                        {
                                using (RabbitmqHelper helper = new RabbitmqHelper(quque.CreateChannel()))
                                {
                                        //绑定交换机
                                        helper.BindExchange(exchanges);
                                        helper.BindExchange(broadcasts);

                                        //为节点创建队列接收广播
                                        string name = string.Concat("Rpc_", _Config.ServerId);
                                        _Queues = new string[]
                                        {
                                                string.Join("_", "Rpc", _Config.SystemVal, _Config.RpcMerId),
                                                string.Join("_", "Rpc",_Config.SysGroupVal, _Config.RpcMerId),
                                                name
                                        };
                                        //绑定队列
                                        helper.QueueDeclare(_Queues);
                                        helper.QueueBind(exchanges, new QueueSubInfo[]
                                        {
                                        new QueueSubInfo
                                        {
                                                //已节点类别创建主队列接收广播
                                                Queue=string.Join("_", "Rpc", _Config.SystemVal,_Config.RpcMerId),
                                                RouteKey=_Config.SystemVal
                                        },
                                         new QueueSubInfo
                                        {
                                                //已节点组创建主队列接收广播
                                                Queue=string.Join("_", "Rpc",_Config.SysGroupVal,_Config.RpcMerId),
                                                RouteKey=_Config.SysGroupVal
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
                                        RouteKey= _Config.SystemVal
                                },
                                new QueueSubInfo
                                {
                                        Queue=name,
                                        RouteKey= _Config.SysGroupVal
                                },
                                new QueueSubInfo
                                {
                                        Queue=name,
                                        RouteKey= "ALLNODE"
                                }
                                        });

                                        string[] subQueue = QueueHelper.GetSubscribQueue(_Config);
                                        helper.QueueDeclare(subQueue);
                                }
                        }
                }
                public static IQueue CreateMsgQueue(SubscribeEvent action)
                {
                        string[] exchanges = QueueHelper.GetExchangeList(_Config, out string[] broadcasts);
                        SubscribBody[] bodys = exchanges.ConvertAll(_Queues, (a, b) => new SubscribBody
                        {
                                Exchange = a,
                                Queue = b
                        });
                        string name = string.Concat("Rpc_", _Config.ServerId);
                        bodys = bodys.Add(broadcasts.ConvertAll(a => new SubscribBody
                        {
                                Exchange = a,
                                Queue = name
                        }));
                        RabbitmqConfig config = new RabbitmqConfig
                        {
                                Config = _Config,
                                IsAutoAck = _Config.IsAutoAck,
                                Subscribs = bodys
                        };
                        return new QueueRabbitmq(config, action);
                }
                public static IQueue CreateSubQueue(SubscribeEvent action)
                {
                        RabbitmqConfig config = new RabbitmqConfig
                        {
                                Config = _Config,
                                IsAutoAck = _Config.SubIsAutoAck,
                                Subscribs = QueueHelper.GetSubscribBody(_Config)
                        };
                        return new QueueRabbitmq(config, action);
                }
        }
}
