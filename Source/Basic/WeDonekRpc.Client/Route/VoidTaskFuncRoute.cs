using System;
using System.Reflection;
using System.Threading.Tasks;
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
    internal class VoidTaskFuncRoute : Route
    {
        private readonly ExecTaskAction _Action;
        public VoidTaskFuncRoute (string name, MethodInfo method) : base(name, method)
        {
            this._Action = RpcRouteHelper.GetExecTaskAction(method);
        }

        protected override IBasicRes _ExecFun (object source, object[] param)
        {
            try
            {
                Task task = this._Action(source, param);
                if (task.Status == TaskStatus.Running)
                {
                    task.Wait();
                }
                if (task.IsCompletedSuccessfully)
                {
                    return ConfigDic.SuccessRes;
                }
                else
                {
                    ErrorException error = ErrorException.FormatError(task.Exception);
                    RpcLogSystem.AddReplyErrorLog(this._Method, this.RouteName, this._SourceType, param, error);
                    return new BasicRes(error.ToString());
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

