using HttpApiGateway.Interface;

namespace HttpApiGateway
{
        internal interface IApiRoute : IRoute
        {
                void ExecApi(IService service);
                bool ReceiveRequest(IService service);
                void CheckUpFormat(IApiService service, string fileName, int num);
                void InitRoute();
                void CheckFileSize(IApiService service, long fileSize);
                bool CheckCache(IApiService service, string etag, string toUpdateTime);
        }
}