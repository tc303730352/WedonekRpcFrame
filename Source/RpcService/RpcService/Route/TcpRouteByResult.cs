using RpcModel;

using RpcService.Model;

namespace RpcService.Route
{

        internal class TcpRoute<T, Result> : ITcpRoute
        {
                public TcpRoute(string name)
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
                protected virtual bool ExecAction(T param, out Result result, out string error)
                {
                        result = default;
                        error = "public.no.func";
                        return false;
                }
                protected virtual bool ExecAction(T param, string ip, out Result result, out string error)
                {
                        return this.ExecAction(param, out result, out error);
                }
                private IBasicRes _MsgHandle(RemoteMsg msg)
                {
                        T data = msg.GetMsgBody<T>();
                        if (data.Equals(default(T)))
                        {
                                return new BasicRes("public.parameter.error");
                        }
                        else if (!this.ExecAction(data, msg.ClientIp, out Result result, out string error))
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
