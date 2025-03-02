using System;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpService.FileUp;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.Scope)]
    public interface IUpFileConfig
    {
        /// <summary>
        /// 上传设置
        /// </summary>
        ApiUpSet UpSet { get; }
        /// <summary>
        /// 上传错误
        /// </summary>
        /// <param name="file"></param>
        /// <param name="e"></param>
        void UpError (IApiService service, Exception e);

        string ApplyTempSavePath (IApiService service, UpFileParam param);
        void InitFileUp (IApiService service);
        void CheckFileSize (IApiService service, long fileSize);

        void CheckUpFormat (IApiService service, string fileName, int num);
    }
}
