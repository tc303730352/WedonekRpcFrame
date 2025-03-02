using System;
using System.Threading.Tasks;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RouteDelegate
{
    internal class VoidTaskFuncDelegate : RouteDelegate
    {
        private readonly ExecTaskAction _Action;
        public VoidTaskFuncDelegate (string name, Delegate source, string show) : base(name, source, show)
        {
            this._Action = RpcRouteHelper.GetExecTaskAction(source.Method);
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
                    RpcLogSystem.AddReplyErrorLog(this._Method, this.RouteName, param, error);
                    return new BasicRes(error.ToString());
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
