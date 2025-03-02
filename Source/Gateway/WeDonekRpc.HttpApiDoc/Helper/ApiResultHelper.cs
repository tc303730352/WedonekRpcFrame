using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.HttpApiGateway.Response;
using System;
namespace WeDonekRpc.HttpApiDoc.Helper
{
    internal class ApiResultHelper
    {
        private static readonly string _JsonType = typeof(JsonResponse).FullName;

        private static readonly string _StreamType = typeof(StreamResponse).FullName;

        private static readonly string _XMLType = typeof(XmlResponse).FullName;

        private static readonly string _HttpStatusType = typeof(HttpStatusResponse).FullName;

        private static readonly string _Redirect = typeof(RedirectResponse).FullName;
        public static ApiReturnType GetReturnType(Type type)
        {
            if (type.IsInterface)
            {
                return ApiReturnType.未知;
            }
            else if (type.BaseType != typeof(object))
            {
                type = type.BaseType;
            }
            string name = type.FullName;
            if (name == _StreamType)
            {
                return ApiReturnType.数据流;
            }
            else if (name == _JsonType)
            {
                return ApiReturnType.Json;
            }
            else if (name == _HttpStatusType)
            {
                return ApiReturnType.HttpStatus;
            }
            else if (name == _XMLType)
            {
                return ApiReturnType.XML;
            }
            else if (name == _XMLType)
            {
                return ApiReturnType.XML;
            }
            else if (name == _Redirect)
            {
                return ApiReturnType.XML;
            }
            return ApiReturnType.未知;
        }
    }
}
