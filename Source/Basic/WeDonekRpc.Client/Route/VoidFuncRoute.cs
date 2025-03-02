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
    /// 无返回值的方法路由
    /// </summary>
    internal class VoidFuncRoute : Route
    {
        private readonly ExecAction _Action;
        public VoidFuncRoute(string name, MethodInfo method) : base(name, method)
        {
            this._Action = RpcRouteHelper.GetExecAction(method);
        }
     
        protected override IBasicRes _ExecFun(object source, object[] param)
        {
            try
            {
                this._Action(source, param);
                return ConfigDic.SuccessRes;
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

