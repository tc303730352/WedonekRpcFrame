using System.Collections.Concurrent;
using System.Collections.Generic;
using WeDonekRpc.Client.Controller;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Kafka;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Kafka;
using WeDonekRpc.Model.Kafka.Model;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// Kafka队列服务
    /// </summary>
    internal class KafkaCollect
    {
        private static string[] _Queues = null;
        /// <summary>
        /// Kafka配置
        /// </summary>
        private static KafkaConfig _Config;
        /// <summary>
        /// 交换机
        /// </summary>
        private static readonly ConcurrentDictionary<string, KafkaExchangeController> _Exchange = new ConcurrentDictionary<string, KafkaExchangeController>();
        public static void Init ()
        {
            _InitQueue(RpcStateCollect.LocalConfig);
            _Config = Config.WebConfig.GetKafkaConfig();
            if (_Config != null)
            {
                AutoClearDic.AutoClear(_Exchange, 600);
            }
        }
        /// <summary>
        /// 获取交换机控制器
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static bool GetExchange (string exchange, out KafkaExchangeController controller)
        {
            if (!_Exchange.TryGetValue(exchange, out controller))
            {
                controller = _Exchange.GetOrAdd(exchange, new KafkaExchangeController(exchange));
            }
            if (!controller.Init())
            {
                if (_Exchange.TryRemove(exchange, out controller))
                {
                    controller.Dispose();
                }
                return false;
            }
            return controller.IsInit;
        }
        private static void _InitQueue (LocalRpcConfig source)
        {
            List<ExchangeDatum> list = [];
            string[] exchanges = QueueHelper.GetExchangeList(source, out string[] broadcasts);

            //为节点创建队列接收广播
            string name = string.Concat("Rpc_", source.ServerId);
            _Queues = new string[]
            {
                    string.Join("_", "Rpc", source.SystemType.Replace(".","-"), source.RpcMerId),
                    string.Join("_", "Rpc",source.SysGroup, source.RpcMerId),
                    name
            };
            exchanges.ForEach(a =>
            {
                list.Add(new ExchangeDatum
                {
                    Exchange = a,
                    RouteKey = new ExchangeRouteKey[]
                    {
                            new ExchangeRouteKey
                            {
                                    //已节点类别创建主队列接收广播
                                    Queue=string.Join("_", "Rpc", source.SystemType.Replace(".","-"),source.RpcMerId),
                                    RouteKey=source.SystemType
                            },
                                    new ExchangeRouteKey
                            {
                                    //已节点组创建主队列接收广播
                                    Queue=string.Join("_", "Rpc",source.SysGroup,source.RpcMerId),
                                    RouteKey=source.SysGroup
                            },
                            new ExchangeRouteKey
                            {
                                    Queue= name,
                                    RouteKey=name
                            },
                            new ExchangeRouteKey
                            {
                                    Queue= name,
                                    RouteKey="ALLNODE"
                            }
                      }
                }); ;
            });
            broadcasts.ForEach(a =>
            {
                list.Add(new ExchangeDatum
                {
                    Exchange = a,
                    RouteKey = new ExchangeRouteKey[]
                    {
                            new ExchangeRouteKey
                            {
                                    Queue = name,
                                    RouteKey = source.SystemType
                            },
                            new ExchangeRouteKey
                            {
                                    Queue = name,
                                    RouteKey = source.SysGroup
                            },
                            new ExchangeRouteKey
                            {
                                    Queue = name,
                                    RouteKey = "ALLNODE"
                            }
                    }
                }); ;
            });
            AddExchange(list.ToArray());
        }
        public static void AddExchange (ExchangeDatum[] exchanges)
        {
            AddKafkaExchange obj = new AddKafkaExchange
            {
                Exchange = exchanges
            };
            if (!RemoteCollect.Send(obj, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public static IQueue CreateSubQueue (SubscribeEvent action)
        {
            if (_Config == null)
            {
                throw new ErrorException("rpc.kafka.config.null");
            }
            SubscribBody[] bodys = QueueHelper.GetSubscribBody(RpcStateCollect.LocalConfig);
            bodys.ForEach(a => a.Queue = _FormatQueue(a.Queue));
            return new QueueKafka(bodys, _Config, action);
        }
        private static string _FormatQueue (string queue)
        {
            if (!queue.Contains('.'))
            {
                return queue;
            }
            return queue.Replace('.', '-');
        }
        public static IQueue CreateMsgQueue (SubscribeEvent action)
        {
            if (_Config == null)
            {
                throw new ErrorException("rpc.kafka.config.null");
            }
            LocalRpcConfig source = RpcStateCollect.LocalConfig;
            string[] exchanges = QueueHelper.GetExchangeList(source, out string[] broadcasts);
            SubscribBody[] bodys = exchanges.ConvertAll(_Queues, (a, b) => new SubscribBody
            {
                Exchange = a,
                Queue = b
            });
            string name = string.Concat("Rpc_", source.ServerId);
            bodys = bodys.Add(broadcasts.ConvertAll(a => new SubscribBody
            {
                Exchange = a,
                Queue = name
            }));
            return new QueueKafka(bodys, _Config, action);
        }
    }
}
