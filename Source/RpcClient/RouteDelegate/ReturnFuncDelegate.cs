using System;

using RpcClient.Config;
using RpcClient.Model;
using RpcClient.Route;

using RpcModel;

using RpcHelper;

namespace RpcClient.RouteDelegate
{
        internal class ReturnFuncDelegate : RouteDelegate
        {
                private readonly bool _IsPaging = false;

                protected readonly int _CountIndex = -1;
                public ReturnFuncDelegate(string name, Delegate source) : base(name, source)
                {
                        this._CountIndex = this._ParamList.FindLastIndex(a => a.ParamType == FuncParamType.数据总数);
                        this._IsPaging = this._CountIndex != -1;
                }

                protected override IBasicRes _ExecFun(object[] param)
                {
                        try
                        {
                                object res = this._Method.Invoke(this._Source, param);
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
                                RpcLogSystem.AddRouteErrorLog(this._Method, this.RouteName, param, ex);
                                return new BasicRes(ex.ToString());
                        }
                }
        }
}

