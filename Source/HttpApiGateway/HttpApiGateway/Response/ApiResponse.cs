using ApiGateway;

using HttpApiGateway.Helper;

namespace HttpApiGateway.Response
{
        internal class ApiResponse : JsonResponse
        {
                public ApiResponse() : base(ApiPublicDict.SuccessJosn)
                {

                }
                public ApiResponse(object data) : base(ApiHelper.GetSuccessJson(data))
                {

                }
                public ApiResponse(object data, object count) : base(ApiHelper.GetSuccessJson(data, count))
                {

                }
        }
}
