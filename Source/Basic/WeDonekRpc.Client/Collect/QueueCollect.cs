
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Queue;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    public enum SubscribeErrorType
    {
        Success = 0,
        NotFind = 1,
        Fail = 2,
        Exclude = 3
    }
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <param name="msg">远程队列消息</param>
    /// <param name="routeKey">路由建</param>
    /// <param name="exchange">交换机</param>
    /// <returns>错误类型</returns>
    public delegate SubscribeErrorType SubscribeEvent (QueueRemoteMsg msg, string routeKey, string exchange);
    /// <summary>
    /// 队列集合
    /// </summary>
    internal class QueueCollect : IQueueCollect
    {
        private readonly IQueue _Queue = null;
        internal QueueCollect (IQueue queue)
        {
            this._Queue = queue;
        }
        public static void InitQueue ()
        {
            RabbitmqCollect.Init();
            KafkaCollect.Init();
        }
        private static IQueue _GetQueue (SubscribeEvent action)
        {
            if (WebConfig.QueueType == QueueType.RabbitMQ)
            {
                return RabbitmqCollect.CreateMsgQueue(action);
            }
            return KafkaCollect.CreateMsgQueue(action);
        }
        private static IQueue _GetSubQueue (SubscribeEvent action)
        {
            if (WebConfig.QueueType == QueueType.RabbitMQ)
            {
                return RabbitmqCollect.CreateSubQueue(action);
            }
            return KafkaCollect.CreateSubQueue(action);
        }

        public static IQueueCollect CreateMsgQueue (SubscribeEvent action)
        {
            return new QueueCollect(_GetQueue(action));
        }
        public static IQueueCollect CreateSubQueue (SubscribeEvent action)
        {
            return new QueueCollect(_GetSubQueue(action));
        }
        public void Dispose ()
        {
            this._Queue.Dispose();
        }
        public void Subscribe ()
        {
            this._Queue.Subscrib();
        }
        public void BindRoute (string routeKey)
        {
            this._Queue.BindRoute(routeKey);
        }

        public bool Public (IRemoteBroadcast config, TcpRemoteMsg msg, long[] serverId)
        {
            string[] routeKey = QueueHelper.GetRouteKey(config, serverId, out string exchange);
            return this._Queue.Public(new QueueRemoteMsg
            {
                Msg = msg,
                TranId = TranService.RpcTranService.TranId,
                Type = config.SysDictate,
                IsExclude = config.IsExclude
            }, routeKey, exchange);
        }

        public bool Public (IRemoteBroadcast config, TcpRemoteMsg msg)
        {
            string[] routeKey = QueueHelper.GetRouteKey(config, out string exchange);
            return this._Queue.Public(new QueueRemoteMsg
            {
                Msg = msg,
                TranId = TranService.RpcTranService.TranId,
                Type = config.SysDictate,
                IsExclude = config.IsExclude
            }, routeKey, exchange);
        }

        public bool Public (IRemoteBroadcast config, TcpRemoteMsg msg, string[] typeVal)
        {
            string[] routeKey = QueueHelper.GetRouteKey(config, config.RpcMerId.GetValueOrDefault(), typeVal, out string exchange);
            return this._Queue.Public(new QueueRemoteMsg
            {
                Msg = msg,
                Type = config.SysDictate,
                TranId = TranService.RpcTranService.TranId,
                IsExclude = config.IsExclude
            }, routeKey, exchange);
        }
        public bool Public (IRemoteBroadcast config, TcpRemoteMsg msg, long rpcMerId, string[] typeVal)
        {
            string[] routeKey = QueueHelper.GetRouteKey(config, rpcMerId, typeVal, out string exchange);
            return this._Queue.Public(new QueueRemoteMsg
            {
                Msg = msg,
                Type = config.SysDictate,
                TranId = TranService.RpcTranService.TranId,
                IsExclude = config.IsExclude
            }, routeKey, exchange);
        }
    }
}
