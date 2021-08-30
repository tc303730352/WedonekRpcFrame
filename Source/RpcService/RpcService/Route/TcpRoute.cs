using RpcModel;

using RpcService.Model;

namespace RpcService.Route
{
        internal class TcpRouteByResult<Result> : ITcpRoute
        {
                public TcpRouteByResult(string name)
                {
                        this.RouteName = name;
                        this.TcpMsgEvent = new TcpMsgEvent(this._MsgHandle);
                }
                /// <summary>
                /// 路由名
                /// </summary>
                public string RouteName
                {
                        get;
                }

                public TcpMsgEvent TcpMsgEvent
                {
                        get;
                }
                protected virtual bool ExecAction(out Result result, out string error)
                {
                        result = default;
                        error = "public.no.func";
                        return false;
                }
                protected virtual bool ExecAction(string ip, out Result result, out string error)
                {
                        return this.ExecAction(out result, out error);
                }
                private IBasicRes _MsgHandle(RemoteMsg msg)
                {
                        if (!this.ExecAction(msg.ClientIp, out Result result, out string error))
                        {
                                return new BasicRes(error);
                        }
                        else
                        {
                                return new BasicRes<Result>(result);
                        }
                }
        }
}
