using System;
using System.Text;
using WeDonekRpc.ApiGateway.Json;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway
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
        private static void _Append (IUserPage page, StringBuilder str)
        {
            if (page.PageId.IsNull())
            {
                _ = str.AppendFormat(",\"direct\":\"{0}\"", page.Direct);
            }
            else
            {
                _ = str.AppendFormat(",\"direct\":\"{0}\",\"id\":\"{1}\"", page.Direct, page.PageId);
            }
        }

        public string AuthorizationFail (ErrorException error)
        {
            return this.GetErrorResponse(_Authorization, error);
        }

        public string AuthorizationSuccess ()
        {
            return this.GetResponse(_Authorization);
        }

        public string GetErrorResponse (IUserPage page, string error)
        {
            if (LocalErrorManage.GetErrorMsg(error, out ErrorMsg msg, out string param))
            {
                StringBuilder json = new StringBuilder("{", 150);
                if (param != null)
                {
                    _ = json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\",\"param\":\"{2}\"", msg.ErrorId, msg.Text, param);
                }
                else
                {
                    _ = json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\"", msg.ErrorId, msg.Text);
                }
                _Append(page, json);
                _ = json.Append("}");
                return json.ToString();
            }
            else
            {
                StringBuilder json = new StringBuilder("{", 150);
                _ = json.AppendFormat("\"errorcode\":-1,\"errmsg\":\"{0}\"", error);
                _Append(page, json);
                _ = json.Append("}");
                return json.ToString();
            }
        }
        public string GetErrorResponse (IUserPage page, ErrorException error)
        {
            ErrorMsg e = error.GetError();
            StringBuilder json = new StringBuilder("{", 150);
            if (error.Param != null)
            {
                _ = json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\",\"param\":\"{2}\"", e.ErrorId, e.Text, error.Param);
            }
            else
            {
                _ = json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\"", e.ErrorId, e.Text);
            }
            _Append(page, json);
            _ = json.Append("}");
            return json.ToString();
        }

        public string GetErrorResponse (ErrorException error)
        {
            return this.GetErrorResponse(_SysError, error);
        }

        public string GetErrorResponse (string error)
        {
            return this.GetErrorResponse(_SysError, error);
        }

        public string GetResponse (IUserPage page)
        {
            StringBuilder json = new StringBuilder("{\"errorcode\":0");
            _Append(page, json);
            _ = json.Append('}');
            return json.ToString();
        }



        public string GetResponse (IUserPage page, object result)
        {
            StringBuilder json = new StringBuilder("{\"errorcode\":0,", 512);
            Type type = result.GetType();
            if (type.Name == PublicDataDic.BoolType.Name)
            {
                _ = json.AppendFormat("\"data\":{0}", (bool)result ? "true" : "false");
            }
            else if (type.Name == PublicDataDic.StringTypeName || type.Name == PublicDataDic.UriTypeName)
            {
                _ = json.AppendFormat("\"data\":\"{0}\"", result);
            }
            else if (type.IsPrimitive)
            {
                _ = json.AppendFormat("\"data\":{0}", result);
            }
            else
            {
                _ = json.AppendFormat("\"data\":{0}", GatewayJsonTools.Json(result));
            }
            _Append(page, json);
            _ = json.Append('}');
            return json.ToString();
        }
    }
}
