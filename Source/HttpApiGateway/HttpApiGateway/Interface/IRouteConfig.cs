using System;

using ApiGateway.Interface;

namespace HttpApiGateway.Interface
{
        public delegate void CheckFile(IApiService service, string fileName, int num);
        public delegate void CheckFileSize(IApiService service, long size);
        public delegate bool CheckCache(IApiService service, string etag, string toUpdateTime);
        public interface IRouteConfig : IApiConfig
        {
                /// <summary>
                /// 授权验证
                /// </summary>
                Action<IApiService> AccreditVer { get; set; }
                /// <summary>
                /// 设置文件上传的委托
                /// </summary>
                /// <param name="checkFile"></param>
                /// <param name="checkSize"></param>
                void SetFileUp(CheckFile checkFile, CheckFileSize checkSize);
                /// <summary>
                /// 设置缓存
                /// </summary>
                /// <param name="cache"></param>
                void SetCacheVer(CheckCache cache);
        }
}