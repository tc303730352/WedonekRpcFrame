using RpcCentral.Service.Interface;
using RpcCentral.Service.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcCentral.Service.Route
{
    internal class TcpRoute<T> : ITcpRoute
    {
        public TcpRoute ()
        {
            this.TcpMsgEvent = new TcpMsgEvent(this._MsgHandle);
        }

        public TcpMsgEvent TcpMsgEvent
        {
            get;
        }
        protected virtual void ExecAction (T param)
        {
            throw new ErrorException("public.no.func");
        }
        protected virtual void ExecAction (T param, string clientIp)
        {
            this.ExecAction(param);
        }
        private IBasicRes _MsgHandle (RemoteMsg msg)
        {
            T data = msg.GetMsgBody<T>();
            if (data.Equals(default(T)))
            {
                return new BasicRes("public.parameter.error");
            }
            try
            {
                this.ExecAction(data, msg.ClientIp);
                return new BasicRes();
            }
            catch (Exception e)
            {
                ErrorException error = ErrorException.FormatError(e);
                error.SaveLog("Rpc");
                return new BasicRes(error.ErrorCode);
            }
        }
    }
}
