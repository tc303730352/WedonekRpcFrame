using System;
using System.Linq;
using System.Reflection;

using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Route
{
        /// <summary>
        /// 方法路由
        /// </summary>
        internal class Route : ITcpRoute
        {
                private readonly bool _IsParam = true;
                private readonly bool _IsStatic = false;

                protected readonly FuncParam[] _ParamList = null;

                protected readonly Type _SourceType = null;

                protected readonly MethodInfo _Method = null;

                public string RouteName
                {
                        get;
                }
                public bool IsSystemRoute
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

                public Route(string name, MethodInfo method)
                {
                        this.IsSystemRoute = method.Module.ScopeName == "RpcClient.dll";
                        ParameterInfo[] param = method.GetParameters();
                        this._ParamList = param.ConvertAll(a => RpcClientHelper.GetParamType(a));
                        this.RouteName = name;
                        this._SourceType = method.DeclaringType;
                        this._IsStatic = method.IsStatic;
                        this._Method = method;
                        this.TcpMsgEvent = new TcpMsgEvent(this._MsgEvent);
                        this._IsParam = this._ParamList.IsExists(a => a.ParamType == FuncParamType.参数 || a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源);
                }
                public virtual bool VerificationRoute()
                {
                        if (this._ParamList.Count(a => a.ParamType == FuncParamType.返回值) > 1)
                        {
                                return false;
                        }
                        return true;
                }
                public override string ToString()
                {
                        return this._Method.ToString();
                }
                private IBasicRes _MsgEvent(IMsg msg)
                {
                        if (!RpcClientHelper.InitParam(msg, this._ParamList, out object[] arg, this._IsParam))
                        {
                                return new BasicRes("public.param.null");
                        }
                        object source = null;
                        if (!this._IsStatic)
                        {
                                source = RpcClient.Unity.Resolve(this._SourceType, this._SourceType.FullName);
                        }
                        return this._ExecFun(source, arg);
                }
                protected virtual IBasicRes _ExecFun(object source, object[] param)
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
                                RpcLogSystem.AddRouteErrorLog(this._Method, this.RouteName, this._SourceType, param, ex);
                                return new BasicRes(ex.ToString());
                        }
                }


        }
}
