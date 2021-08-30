using System;
using System.Reflection;

using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.Route;

using RpcModel;

using RpcHelper;

namespace RpcClient.RouteDelegate
{
        internal class RouteDelegate : ITcpRoute
        {

                private readonly bool _IsParam = true;

                protected readonly FuncParam[] _ParamList = null;

                protected readonly Delegate _Source = null;

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
                public RouteDelegate(string name, Delegate func)
                {
                        this._Source = func;
                        this._Method = func.Method;
                        this.IsSystemRoute = func.Method.Module.ScopeName == "RpcClient.dll";
                        ParameterInfo[] param = this._Method.GetParameters();
                        this._ParamList = param.ConvertAll(a => RpcClientHelper.GetParamType(a));
                        this.RouteName = name;
                        this.TcpMsgEvent = new TcpMsgEvent(this._MsgEvent);
                        this._IsParam = this._ParamList.IsExists(a => a.ParamType == FuncParamType.参数 || a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源);
                }
                public override string ToString()
                {
                        return this._Method.ToString();
                }
                public virtual bool VerificationRoute()
                {
                        return true;
                }

                private IBasicRes _MsgEvent(IMsg msg)
                {
                        if (!RpcClientHelper.InitParam(msg, this._ParamList, out object[] arg, this._IsParam))
                        {
                                return new BasicRes("public.param.null");
                        }
                        return this._ExecFun(arg);
                }
                protected virtual IBasicRes _ExecFun(object[] param)
                {
                        try
                        {
                                object res = this._Method.Invoke(this._Source, param);
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
                                RpcLogSystem.AddRouteErrorLog(this._Method, this.RouteName, param, ex);
                                return new BasicRes(ex.ToString());
                        }
                }
        }
}
