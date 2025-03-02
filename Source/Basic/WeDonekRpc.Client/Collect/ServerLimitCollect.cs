using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.RpcApi;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;
using WeDonekRpc.Client.Server;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// 节点限流服务
    /// </summary>
    internal class ServerLimitCollect
    {
        private static ILimitServer _Limit = null;
        static ServerLimitCollect()
        {
            RpcClient.Route.AddRoute("RefreshLimit", _Refresh);
        }
        /// <summary>
        /// 刷新配置
        /// </summary>
        private static void _Refresh()
        {
            ILimitServer limit = _GetLimit();
            if (_Limit != null)
            {
                _Limit.Dispose();
            }
            _Limit = limit;
        }

        internal static TcpRemoteReply MsgEvent(string type, TcpRemoteMsg msg, IIOClient client)
        {
            return _Limit.MsgEvent(type, msg, client);
        }
        /// <summary>
        /// 初始化节点限流配置
        /// </summary>
        public static void Init()
        {
            _Limit = _GetLimit();
        }
        /// <summary>
        /// 获取最新的限流器
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ErrorException"></exception>
        private static ILimitServer _GetLimit()
        {
            if (!RpcTokenCollect.GetAccessToken(out RpcToken token, out string error))
            {
                throw new ErrorException(error);
            }
            else if (!RpcServiceApi.GetServerLimit(token, out LimitConfigRes config, out error))
            {
                throw new ErrorException(error);
            }
            else
            {
                return new Limit.ServerLimit(config);
            }
        }

    }
}
