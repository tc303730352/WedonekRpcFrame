using System.Collections.Generic;
using System.Net;

namespace WeDonekRpc.HttpApiGateway.Config
{
    internal class ApiHttpErrorDic
    {
        private static readonly Dictionary<HttpStatusCode, string> _StatusDic = new Dictionary<HttpStatusCode, string>();

        static ApiHttpErrorDic()
        {
            _StatusDic.Add(HttpStatusCode.NotFound, "http.404");
            _StatusDic.Add(HttpStatusCode.InternalServerError, "http.500");
        }
        public static string GetErrorCode(HttpStatusCode code)
        {
            return _StatusDic[code];
        }
    }
}
