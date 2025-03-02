using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// 广播服务
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
        /// <summary>
        /// 将广播特性配置转换为特定广播服务
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ErrorException"></exception>
        public static IBroadcast GetBroadcast (IRemoteBroadcast config)
        {
            IBroadcast bro = _GetBroadcast(config);
            if (bro == null)
            {
                throw new ErrorException("rpc.queue.no.config");
            }
            return bro;
        }
        private static IBroadcast _GetBroadcast (IRemoteBroadcast config)
        {
            config.InitBroadcast();
            if (config.BroadcastType == BroadcastType.消息队列)
            {
                return _Queue;
            }
            else if (config.BroadcastType == BroadcastType.订阅)
            {
                return _Subscribe;
            }
            else if (RpcTranService.SourceTran != null)
            {
                return _Queue;
            }
            return _Local;
        }

        public static void Init ()
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
