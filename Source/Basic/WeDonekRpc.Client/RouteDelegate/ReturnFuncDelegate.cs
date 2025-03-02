using System;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RouteDelegate
{
    internal class ReturnFuncDelegate : RouteDelegate
    {
        private readonly ExecFunc _Action;
        public ReturnFuncDelegate (string name, Delegate source, string show) : base(name, source, show)
        {
            this._Action = RpcRouteHelper.GetExecFunc(source.Method);
        }

        protected override IBasicRes _ExecFun (object source, object[] param)
        {
            try
            {
                object res = this._Action(source, param);
                if (res != null)
                {
                    return new BasicResObj(res);
                }
                else
                {
                    return ConfigDic.SuccessRes;
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

