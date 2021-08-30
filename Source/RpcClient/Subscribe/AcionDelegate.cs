using System;
using System.Reflection;

using RpcClient.Collect;
using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.Route;

using RpcHelper;

namespace RpcClient.Subscribe
{
        internal class AcionDelegate : ISubscribeEvent
        {
                private readonly bool _IsParam = true;

                protected readonly FuncParam[] _ParamList = null;

                protected readonly Delegate _Source = null;

                protected readonly MethodInfo _Method = null;

                public string EventName { get; }

                public AcionDelegate(string name, Delegate func)
                {
                        this._Source = func;
                        this._Method = func.Method;
                        ParameterInfo[] param = this._Method.GetParameters();
                        this._ParamList = param.ConvertAll(a => RpcClientHelper.GetParamType(a));
                        this.EventName = name;
                        this._IsParam = this._ParamList.IsExists(a => a.ParamType == FuncParamType.参数 || a.ParamType == FuncParamType.数据源 || a.ParamType == FuncParamType.源);
                }
                public virtual bool VerificationRoute()
                {
                        return true;
                }

                private bool _ExecFun(object[] param)
                {
                        try
                        {
                                this._Method.Invoke(this._Source, param);
                                return true;
                        }
                        catch (Exception e)
                        {
                                RpcLogSystem.AddRouteErrorLog(this._Method, this.EventName, param, ErrorException.FormatError(e));
                                return false;
                        }
                }

                public bool Exec(IMsg msg)
                {
                        if (!RpcClientHelper.InitParam(msg, this._ParamList, out object[] arg, this._IsParam))
                        {
                                return false;
                        }
                        return this._ExecFun(arg);
                }

                public bool Init()
                {
                        RpcSubscribeCollect.BindRoute(this.EventName);
                        return true;
                }
        }
}
