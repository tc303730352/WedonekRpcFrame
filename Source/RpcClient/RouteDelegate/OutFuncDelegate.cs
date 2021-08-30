using System;

using RpcClient.Model;
using RpcClient.Route;

using RpcModel;

using RpcHelper;
namespace RpcClient.RouteDelegate
{
        internal class OutFuncDelegate : RouteDelegate
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
                public OutFuncDelegate(string name, Delegate source) : base(name, source)
                {
                        this._ErrorIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.错误信息);
                        this._ReturnIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.返回值);
                        this._CountIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsPaging = this._CountIndex != -1;
                }

                protected override IBasicRes _ExecFun(object[] param)
                {
                        try
                        {
                                if (!(bool)this._Method.Invoke(this._Source, param))
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
                                RpcLogSystem.AddRouteErrorLog(this._Method, this.RouteName, param, ex);
                                return new BasicRes(ex.ToString());
                        }
                }
        }
}