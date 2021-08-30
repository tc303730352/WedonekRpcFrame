using RpcModel;

using RpcService.Model;

namespace RpcService.Route
{
        internal class TcpRouteByVoid<T> : ITcpRoute
        {
                public TcpRouteByVoid(string name)
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
                protected virtual void ExecAction(T param)
                {
                }
                private IBasicRes _MsgHandle(RemoteMsg msg)
                {
                        T data = msg.GetMsgBody<T>();
                        if (data == null || data.Equals(default(T)))
                        {
                                return new BasicRes("public.parameter.error");
                        }
                        this.ExecAction(data);
                        return null;
                }
        }
}

