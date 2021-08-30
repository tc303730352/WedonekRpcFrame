using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Queue.Model;

using RpcModel;

namespace RpcClient.Collect
{
        public delegate bool SubscribeEvent(QueueRemoteMsg msg, string routeKey, string exchange);
        internal class QueueCollect : IQueueCollect
        {
                private readonly IQueue _Queue = null;
                internal QueueCollect(IQueue queue)
                {
                        this._Queue = queue;
                }
                public static void InitQueue(QueueConfig config)
                {
                        RabbitmqCollect.Init(config);
                }
                public static IQueueCollect CreateMsgQueue(SubscribeEvent action)
                {
                        return new QueueCollect(RabbitmqCollect.CreateMsgQueue(action));
                }
                public static IQueueCollect CreateSubQueue(SubscribeEvent action)
                {
                        return new QueueCollect(RabbitmqCollect.CreateSubQueue(action));
                }
                public void Dispose()
                {
                        this._Queue.Dispose();
                }
                public void Subscribe()
                {
                        this._Queue.Subscrib();
                }
                public void BindRoute(string routeKey)
                {
                        this._Queue.BindRoute(routeKey);
                }

                public bool Public(IRemoteBroadcast config, TcpRemoteMsg msg, long[] serverId)
                {
                        string[] routeKey = QueueHelper.GetRouteKey(config, serverId, out string exchange);
                        return this._Queue.Public(new QueueRemoteMsg
                        {
                                Msg = msg,
                                Type = config.SysDictate,
                                IsExclude = config.IsExclude
                        }, routeKey, exchange);
                }

                public bool Public(IRemoteBroadcast config, TcpRemoteMsg msg)
                {
                        string[] routeKey = QueueHelper.GetRouteKey(config, out string exchange);
                        return this._Queue.Public(new QueueRemoteMsg
                        {
                                Msg = msg,
                                Type = config.SysDictate,
                                IsExclude = config.IsExclude
                        }, routeKey, exchange);
                }

                public bool Public(IRemoteBroadcast config, TcpRemoteMsg msg, string[] typeVal)
                {
                        string[] routeKey = QueueHelper.GetRouteKey(config, config.RpcMerId, typeVal, out string exchange);
                        return this._Queue.Public(new QueueRemoteMsg
                        {
                                Msg = msg,
                                Type = config.SysDictate,
                                IsExclude = config.IsExclude
                        }, routeKey, exchange);
                }
                public bool Public(IRemoteBroadcast config, TcpRemoteMsg msg, long rpcMerId, string[] typeVal)
                {
                        string[] routeKey = QueueHelper.GetRouteKey(config, rpcMerId, typeVal, out string exchange);
                        return this._Queue.Public(new QueueRemoteMsg
                        {
                                Msg = msg,
                                Type = config.SysDictate,
                                IsExclude = config.IsExclude
                        }, routeKey, exchange);
                }
        }
}
