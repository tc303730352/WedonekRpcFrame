using HttpApiGateway.Helper;

namespace HttpApiGateway.Response
{
        /// <summary>
        /// Api网关错误响应
        /// </summary>
        public class ApiErrorResponse : JsonResponse
        {
                public ApiErrorResponse(string error) : base(ApiHelper.GetErrorJson(error))
                {

                }

        }
}
