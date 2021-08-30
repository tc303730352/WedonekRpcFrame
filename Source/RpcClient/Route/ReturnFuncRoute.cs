using System;
using System.Reflection;

using RpcClient.Config;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Route
{
        /// <summary>
        /// 
        /// </summary>
        internal class ReturnFuncRoute : Route
        {
                private readonly bool _IsPaging = false;

                protected readonly int _CountIndex = -1;
                public ReturnFuncRoute(string name, MethodInfo method) : base(name, method)
                {
                        this._CountIndex = this._ParamList.FindLastIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsPaging = this._CountIndex != -1;
                }
                public override bool VerificationRoute()
                {
                        if (!base.VerificationRoute())
                        {
                                return false;
                        }
                        else if (this._ParamList.IsExists(a => a.ParamType == FuncParamType.错误信息))
                        {
                                return false;
                        }
                        else if (!this._IsPaging)
                        {
                                return true;
                        }
                        Type type = this._Method.ReturnType;
                        return type.IsArray || type.IsGenericType;
                }
                protected override IBasicRes _ExecFun(object source, object[] param)
                {
                        try
                        {
                                object res = this._Method.Invoke(source, param);
                                if (this._IsPaging)
                                {
                                        return new BasicPagingRes
                                        {
                                                Count = param[this._CountIndex],
                                                DataList = res
                                        };
                                }
                                else if (res != null)
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
                                RpcLogSystem.AddRouteErrorLog(this._Method, this.RouteName, this._SourceType, param, ex);
                                return new BasicRes(ex.ToString());
                        }
                }
        }
}
