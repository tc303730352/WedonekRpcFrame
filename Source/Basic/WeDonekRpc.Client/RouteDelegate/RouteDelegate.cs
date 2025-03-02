using System;
using System.Reflection;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Route;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RouteDelegate
{
    internal abstract class RouteDelegate : ITcpRoute
    {

        private readonly bool _IsParam = true;
        private static readonly IIocService _Ioc = RpcClient.Ioc;
        protected readonly FuncParam[] _ParamList = null;

        protected readonly MethodInfo _Method = null;

        public bool IsSystemRoute { get; }

        public string RouteName
        {
            get;
        }

        public TcpMsgEvent TcpMsgEvent
        {
            get;
        }
        private string _Show = null;
        public string RouteShow
        {
            get
            {
                if (this._Show == null)
                {
                    this._Show = XmlShowHelper.FindParamShow(this._Method);
                }
                return this._Show;
            }
        }

        public MethodInfo Source => this._Method;

        public RouteDelegate (string name, Delegate func)
        {
            this._Method = func.Method;
            this.IsSystemRoute = func.Method.Module.ScopeName == "WeDonekRpc.Client.dll";
            ParameterInfo[] param = this._Method.GetParameters();
            this._ParamList = param.ConvertAll(a => RpcClientHelper.GetParamType(a));
            this.RouteName = name;
            this.TcpMsgEvent = new TcpMsgEvent(this._MsgEvent);
            this._IsParam = this._ParamList.IsExists(a => a.ParamType == FuncParamType.参数 || a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源);
        }
        public RouteDelegate (string name, Delegate func, string show) : this(name, func)
        {
            this._Show = show;
        }
        public override string ToString ()
        {
            return this._Method.ToString();
        }
        public virtual bool VerificationRoute ()
        {
            return true;
        }

        private IBasicRes _MsgEvent (IMsg msg)
        {
            using (IocScope scope = _Ioc.CreateScore())
            {
                if (!RpcClientHelper.InitParam(msg, this._ParamList, out object[] arg, this._IsParam))
                {
                    return new BasicRes("public.param.null");
                }
                else
                {
                    return this._ExecFun(null, arg);
                }
            }
        }
        protected virtual IBasicRes _ExecFun (object source, object[] param)
        {
            try
            {
                object res = this._Method.Invoke(source, param);
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
