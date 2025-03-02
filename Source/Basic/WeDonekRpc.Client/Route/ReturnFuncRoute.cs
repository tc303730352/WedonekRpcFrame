using System;
using System.Reflection;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Route
{
    /// <summary>
    /// 
    /// </summary>
    internal class ReturnFuncRoute : Route
    {
        private readonly ExecFunc _Action;
        public ReturnFuncRoute(string name, MethodInfo method) : base(name, method)
        {
            this._Action = RpcRouteHelper.GetExecFunc(method);
        }
        protected override IBasicRes _ExecFun(object source, object[] param)
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
                RpcLogSystem.AddReplyErrorLog(this._Method, this.RouteName, this._SourceType, param, ex);
                return new BasicRes(ex.ToString());
            }
        }
    }
}
