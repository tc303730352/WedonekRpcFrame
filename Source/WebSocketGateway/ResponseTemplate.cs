using System;
using System.Text;

using RpcHelper;

using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway
{
        internal class ResponseTemplate : IResponseTemplate
        {
                private static readonly IUserPage _SysError = new UserPage
                {
                        Direct = "SystemError"
                };
                private static readonly IUserPage _Authorization = new UserPage
                {
                        Direct = "Authorization"
                };
                private static void _Append(IUserPage page, StringBuilder str)
                {
                        if (page.PageId.IsNull())
                        {
                                str.AppendFormat(",\"direct\":\"{0}\"", page.Direct);
                        }
                        else
                        {
                                str.AppendFormat(",\"direct\":\"{0}\",\"id\":\"{1}\"", page.Direct, page.PageId);
                        }
                }

                public string AuthorizationFail(ErrorException error)
                {
                        return this.GetErrorResponse(_Authorization, error);
                }

                public string AuthorizationSuccess()
                {
                        return this.GetResponse(_Authorization);
                }

                public string GetErrorResponse(IUserPage page, string error)
                {
                        if (ErrorManage.GetErrorMsg(error, out ErrorMsg msg, out string param))
                        {
                                StringBuilder json = new StringBuilder("{", 150);
                                if (param != null)
                                {
                                        json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\",\"param\":\"{2}\"", msg.ErrorId, msg.Msg, param);
                                }
                                else
                                {
                                        json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\"", msg.ErrorId, msg.Msg);
                                }
                                _Append(page, json);
                                json.Append("}");
                                return json.ToString();
                        }
                        else
                        {
                                StringBuilder json = new StringBuilder("{", 150);
                                json.AppendFormat("\"errorcode\":-1,\"errmsg\":\"{0}\"", error);
                                _Append(page, json);
                                json.Append("}");
                                return json.ToString();
                        }
                }
                public string GetErrorResponse(IUserPage page, ErrorException error)
                {
                        ErrorMsg e = error.GetError();
                        StringBuilder json = new StringBuilder("{", 150);
                        if (error.Param != null)
                        {
                                json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\",\"param\":\"{2}\"", e.ErrorId, e.Msg, error.Param);
                        }
                        else
                        {
                                json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\"", e.ErrorId, e.Msg);
                        }
                        _Append(page, json);
                        json.Append("}");
                        return json.ToString();
                }

                public string GetErrorResponse(ErrorException error)
                {
                        return this.GetErrorResponse(_SysError, error);
                }

                public string GetResponse(IUserPage page)
                {
                        StringBuilder json = new StringBuilder("{\"errorcode\":0");
                        _Append(page, json);
                        json.Append('}');
                        return json.ToString();
                }

                public string GetResponse(IUserPage page, object result, object count)
                {
                        StringBuilder json = new StringBuilder("{\"errorcode\":0,\"data\":{", 1024);
                        json.AppendFormat("\"list\":{0},\"count\":{1}", result.ToJson(), count);
                        json.Append('}');
                        _Append(page, json);
                        json.Append('}');
                        return json.ToString();
                }

                public string GetResponse(IUserPage page, object result)
                {
                        StringBuilder json = new StringBuilder("{\"errorcode\":0,", 512);
                        Type type = result.GetType();
                        if (type.Name == PublicDataDic.BoolType.Name)
                        {
                                json.AppendFormat("\"data\":{0}", (bool)result ? "true" : "false");
                        }
                        else if (type.Name == PublicDataDic.StringTypeName || type.Name == PublicDataDic.UriTypeName)
                        {
                                json.AppendFormat("\"data\":\"{0}\"", result);
                        }
                        else if (type.IsPrimitive)
                        {
                                json.AppendFormat("\"data\":{0}", result);
                        }
                        else
                        {
                                json.AppendFormat("\"data\":{0}", Tools.Json(result));
                        }
                        _Append(page, json);
                        json.Append('}');
                        return json.ToString();
                }
        }
}
