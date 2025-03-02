using RpcCentral.Service.Interface;
using RpcCentral.Service.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcCentral.Service.Route
{
    internal class TcpRouteByResult<Result> : ITcpRoute
    {
        public TcpRouteByResult()
        {
            TcpMsgEvent = new TcpMsgEvent(this._MsgHandle);
        }
     
        public TcpMsgEvent TcpMsgEvent
        {
            get;
        }
        protected virtual Result ExecAction()
        {
            throw new ErrorException("public.no.func");
        }
        protected virtual Result ExecAction(string ip)
        {
            return ExecAction();
        }
        private IBasicRes _MsgHandle(RemoteMsg msg)
        {
            try
            {
                Result result = ExecAction(msg.ClientIp);
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
