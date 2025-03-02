using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Response
{
    /// <summary>
    /// Api网关响应模板
    /// </summary>
    internal class ApiResponseTemplate : IApiResponseTemplate
    {
        public IResponse GetErrorResponse(string error)
        {
            return new ApiErrorResponse(error);
        }

        public IResponse GetResponse()
        {
            return new ApiResponse();
        }



        public IResponse GetResponse(object result)
        {
            return new ApiResponse(result);
        }
    }
}
