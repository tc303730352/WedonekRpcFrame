using RpcClient.Interface;
using RpcClient.RpcApi;

using RpcModel;
using RpcModel.Model;

using SocketTcpServer.Interface;

using RpcHelper;

namespace RpcClient.Collect
{
        internal class ServerLimitCollect
        {
                private static ILimitServer _Limit = null;
                static ServerLimitCollect()
                {
                        RpcClient.Route.AddRoute("RefreshLimit", _Refresh);
                }
                private static void _Refresh()
                {
                        if (!_Init(out ILimitServer limit, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        if (_Limit != null)
                        {
                                _Limit.Dispose();
                        }
                        _Limit = limit;
                }

                internal static TcpRemoteReply MsgEvent(string type, TcpRemoteMsg msg, ISocketClient client)
                {
                        return _Limit.MsgEvent(type, msg, client);
                }

                public static bool Init(out string error)
                {
                        if (!_Init(out ILimitServer limit, out error))
                        {
                                return false;
                        }
                        _Limit = limit;
                        return true;
                }
                private static bool _Init(out ILimitServer limit, out string error)
                {
                        if (!RpcTokenCollect.GetAccessToken(out RpcToken token, out error))
                        {
                                limit = null;
                                return false;
                        }
                        else if (!RpcServiceApi.GetServerLimit(token, out LimitConfigRes config, out error))
                        {
                                limit = null;
                                return false;
                        }
                        else
                        {
                                limit = new Limit.ServerLimit(config);
                                return true;
                        }
                }

        }
}
