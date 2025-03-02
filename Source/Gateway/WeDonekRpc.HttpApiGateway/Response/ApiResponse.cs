using WeDonekRpc.ApiGateway;

using WeDonekRpc.HttpApiGateway.Helper;

namespace WeDonekRpc.HttpApiGateway.Response
{
    internal class ApiResponse : JsonResponse
    {
        public ApiResponse() : base(ApiPublicDict.SuccessJosn)
        {

        }
        public ApiResponse(object data) : base(ApiHelper.GetSuccessJson(data))
        {

        }
    }
}
