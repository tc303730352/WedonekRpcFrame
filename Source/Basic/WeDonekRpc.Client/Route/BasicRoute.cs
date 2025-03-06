using System;
using System.Reflection;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.RouteService;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Route
{
    internal class BasicRoute : Route
    {
        private readonly ExecRoute _Action;
        public BasicRoute ( string name, MethodInfo method ) : base(name, method)
        {
            this._Action = RpcRouteHelper.GetExecRoute(method);
        }

        protected override IBasicRes _ExecFun ( object source, object[] param )
        {
            try
            {
                IBasicRes res = this._Action(source, param);
                if ( res == null )
                {
                    return new BasicRes("rpc.result.null");
                }
                else
                {
                    return res;
                }
            }
            catch ( Exception e )
            {
                ErrorException ex = ErrorException.FormatError(e);
                RpcLogSystem.AddReplyErrorLog(this._Method, this.RouteName, this._SourceType, param, ex);
                return new BasicRes(ex.ToString());
            }
        }


    }
}
