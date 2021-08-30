using RpcClient.Interface;

using RpcModel;

using RpcHelper;
namespace RpcClient.Collect
{
        /// <summary>
        /// 广播集
        /// </summary>
        internal class BroadcastCollect
        {
                /// <summary>
                /// 框架广播服务
                /// </summary>
                private static IBroadcast _Local = null;
                /// <summary>
                /// 队列广播服务
                /// </summary>
                private static IBroadcast _Queue = null;
                /// <summary>
                /// 订阅服务
                /// </summary>
                private static IBroadcast _Subscribe = null;

                public static IBroadcast GetBroadcast(IRemoteBroadcast config)
                {
                        IBroadcast bro = _GetBroadcast(config);
                        if (bro == null)
                        {
                                throw new ErrorException("rpc.queue.no.config");
                        }
                        return bro;
                }
                private static IBroadcast _GetBroadcast(IRemoteBroadcast config)
                {
                        if (config.BroadcastType == BroadcastType.消息)
                        {
                                return _Queue;
                        }
                        else if (config.BroadcastType == BroadcastType.订阅)
                        {
                                return _Subscribe;
                        }
                        else if (RpcTranCollect.Tran != null)
                        {
                                return _Queue;
                        }
                        return _Local;
                }

                public static void Init()
                {
                        _Local = new RpcMsgCollect();
                        if (Config.WebConfig.RpcConfig.IsEnableQueue)
                        {
                                RpcQueueCollect.InitQueue();
                                RpcSubscribeCollect.Init();
                                _Queue = new RpcQueueCollect();
                                _Subscribe = new RpcSubscribeCollect();
                        }
                }
        }
}
