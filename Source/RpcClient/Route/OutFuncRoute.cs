using System;
using System.Reflection;

using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Route
{

        /// <summary>
        /// 使用返回BOOl的方法解释器
        /// </summary>
        internal class OutFuncRoute : Route
        {
                /// <summary>
                /// 是否分页
                /// </summary>
                private readonly bool _IsPaging = false;

                /// <summary>
                /// 错误信息参数索引位
                /// </summary>
                private readonly int _ErrorIndex = 0;
                /// <summary>
                /// 返回值参数索引位
                /// </summary>
                private readonly int _ReturnIndex = 0;
                /// <summary>
                /// 返回数据量参数索引位
                /// </summary>
                private readonly int _CountIndex = 0;

                public OutFuncRoute(string name, MethodInfo method) : base(name, method)
                {
                        this._ErrorIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.错误信息);
                        this._ReturnIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.返回值);
                        this._CountIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsPaging = this._CountIndex != -1;
                }

                public override bool VerificationRoute()
                {
                        if (!base.VerificationRoute())
                        {
                                return false;
                        }
                        else if (this._ErrorIndex == -1)
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
                                if (!(bool)this._Method.Invoke(source, param))
                                {
                                        return new BasicRes(param[this._ErrorIndex].ToString());
                                }
                                else if (this._IsPaging)
                                {
                                        return new BasicPagingRes(param[this._ReturnIndex], param[this._CountIndex]);
                                }
                                else if (this._ReturnIndex == -1)
                                {
                                        return Config.ConfigDic.SuccessRes;
                                }
                                else
                                {
                                        return new BasicResObj(param[this._ReturnIndex]);
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

