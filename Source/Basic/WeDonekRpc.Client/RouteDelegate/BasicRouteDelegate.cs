using System;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RouteDelegate
{
    /// <summary>
    /// 待测试可能存在问题
    /// </summary>
    internal class BasicRouteDelegate : RouteDelegate
    {
        private readonly ExecRoute _Action;
        public BasicRouteDelegate (string name, Delegate source, string show) : base(name, source, show)
        {
            this._Action = RpcRouteHelper.GetExecRoute(source.Method);
        }
        protected override IBasicRes _ExecFun (object source, object[] param)
        {
            try
            {
                object res = this._Action(source, param);
                if (res == null)
                {
                    return new BasicRes("rpc.result.null");
                }
                else
                {
                    return (IBasicRes)res;
                }
            }
            catch (Exception e)
            {
                ErrorException ex = ErrorException.FormatError(e);
                RpcLogSystem.AddReplyErrorLog(this._Method, this.RouteName, param, ex);
                return new BasicRes(ex.ToString());
            }
        }
    }
}
