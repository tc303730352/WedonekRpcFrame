using RpcCentral.Service.Interface;
using RpcCentral.Service.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcCentral.Service.Route
{

    internal class TcpRoute<T, Result> : ITcpRoute
    {
        public TcpRoute()
        {
            TcpMsgEvent = new TcpMsgEvent(this._MsgHandle);
        }
     
        public TcpMsgEvent TcpMsgEvent
        {
            get;
        }
        protected virtual Result ExecAction(T param)
        {
            throw new ErrorException("public.no.func");
        }
        protected virtual Result ExecAction(T param, string ip)
        {
            return ExecAction(param);
        }
        private IBasicRes _MsgHandle(RemoteMsg msg)
        {
            T data = msg.GetMsgBody<T>();
            if (data.Equals(default(T)))
            {
                return new BasicRes("public.parameter.error");
            }
            try
            {
                Result result = ExecAction(data, msg.ClientIp);
                return new BasicRes<Result>(result);
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
