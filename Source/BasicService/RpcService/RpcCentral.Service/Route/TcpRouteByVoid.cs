using RpcCentral.Service.Interface;
using RpcCentral.Service.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcCentral.Service.Route
{
    internal class TcpRouteByVoid<T> : ITcpRoute
    {
        public TcpRouteByVoid()
        {
            TcpMsgEvent = new TcpMsgEvent(this._MsgHandle);
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
            try
            {
                ExecAction(data);
                return null;
            }
            catch (Exception e)
            {
                ErrorException error = ErrorException.FormatError(e);
                error.SaveLog("Rpc");
                return null;
            }
        }
    }
}

