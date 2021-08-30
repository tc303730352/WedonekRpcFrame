using System;
using System.Reflection;

using RpcClient.Config;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Route
{
        /// <summary>
        /// 无返回值的方法路由
        /// </summary>
        internal class VoidFuncRoute : Route
        {
                private readonly bool _IsPaging = false;
                protected readonly int _ReturnIndex = -1;

                protected readonly int _CountIndex = -1;
                public VoidFuncRoute(string name, MethodInfo method) : base(name, method)
                {
                        this._CountIndex = this._ParamList.FindLastIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsPaging = this._CountIndex != -1;
                        this._ReturnIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.返回值);
                }
                public override bool VerificationRoute()
                {
                        if (!base.VerificationRoute())
                        {
                                return false;
                        }
                        else if (!this._IsPaging)
                        {
                                return true;
                        }
                        else if (this._ReturnIndex == -1)
                        {
                                return false;
                        }
                        Type type = this._ParamList[this._ReturnIndex].DataType;
                        return type.IsArray || type.IsGenericType;
                }
                protected override IBasicRes _ExecFun(object source, object[] param)
                {
                        try
                        {
                                this._Method.Invoke(source, param);
                                if (this._IsPaging)
                                {
                                        return new BasicPagingRes
                                        {
                                                Count = param[this._CountIndex],
                                                DataList = param[this._ReturnIndex]
                                        };
                                }
                                else if (this._ReturnIndex != -1)
                                {
                                        return new BasicResObj(param[this._ReturnIndex]);
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

