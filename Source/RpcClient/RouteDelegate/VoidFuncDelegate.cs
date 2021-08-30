using System;

using RpcClient.Config;
using RpcClient.Model;
using RpcClient.Route;

using RpcModel;

using RpcHelper;

namespace RpcClient.RouteDelegate
{
        internal class VoidFuncDelegate : RouteDelegate
        {
                private readonly bool _IsPaging = false;
                protected readonly int _ReturnIndex = -1;

                protected readonly int _CountIndex = -1;
                public VoidFuncDelegate(string name, Delegate source) : base(name, source)
                {
                        this._CountIndex = this._ParamList.FindLastIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsPaging = this._CountIndex != -1;
                        this._ReturnIndex = this._ParamList.FindIndex(a => a.ParamType == FuncParamType.返回值);
                }

                protected override IBasicRes _ExecFun(object[] param)
                {
                        try
                        {
                                this._Method.Invoke(this._Source, param);
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
                                RpcLogSystem.AddRouteErrorLog(this._Method, this.RouteName, param, ex);
                                return new BasicRes(ex.ToString());
                        }
                }
        }
}
