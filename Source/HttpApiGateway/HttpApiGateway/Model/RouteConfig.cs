using System;

using ApiGateway.Model;

using HttpApiGateway.Interface;

namespace HttpApiGateway.Model
{
        internal class RouteConfig : ApiConfig, IRouteConfig
        {
                public Action<IApiService> AccreditVer { get; set; }

                public CheckCache Cache { get; private set; }

                public CheckFile CheckFile { get; private set; }

                public CheckFileSize CheckSize { get; private set; }
                public void SetCacheVer(CheckCache cache)
                {
                        this.Cache = cache;
                }

                public void SetFileUp(CheckFile checkFile, CheckFileSize checkSize)
                {
                        this.CheckFile = checkFile;
                        this.CheckSize = checkSize;
                }
        }
}
