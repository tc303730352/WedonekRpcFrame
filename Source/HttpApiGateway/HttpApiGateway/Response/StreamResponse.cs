using System.IO;

using HttpApiGateway.File;
using HttpApiGateway.Interface;
using HttpApiGateway.Model;

using HttpService.Helper;

namespace HttpApiGateway.Response
{
        public sealed class StreamResponse : IResponse
        {
                private readonly IResponseStream _Stream = null;
                public StreamResponse(string filePath)
                {
                        this._Stream = new ResponseFile(filePath);
                }
                public StreamResponse(Stream stream, string fileName)
                {
                        this._Stream = new ResponseStream(stream, fileName);
                }
                public StreamResponse(byte[] stream, string fileName)
                {
                        this._Stream = new ResponseStream(stream, fileName);
                }
                public StreamResponse(FileInfo file)
                {
                        this._Stream = new ResponseFile(file);
                }
                public bool Verification(IApiService service)
                {
                        if (!this._Stream.IsExists)
                        {
                                service.Response.SetHttpStatus(System.Net.HttpStatusCode.NotFound);
                                return false;
                        }
                        return true;
                }
                public bool IsBinary
                {
                        get;
                        set;
                }
                public CacheSet Cache
                {
                        get;
                        set;
                }
                public ResponseType ResponseType => ResponseType.Stream;

                public void InitResponse(IApiService service)
                {
                        if (this.IsBinary)
                        {
                                service.Response.ContentType = "application/octet-stream";
                                service.Response.SetHead("Content-Disposition", string.Concat("attachment;filename=", this._Stream.FileName));
                                service.Response.SetHead("Content-Transfer-Encoding", "binary");
                        }
                        else
                        {
                                service.Response.ContentType = RequestHeader.GetHeader(this._Stream.Extension);
                                if (this.Cache == null)
                                {
                                        return;
                                }
                                else if (this.Cache.IsNoCache)
                                {
                                        service.Response.SetCacheType("no-cache");
                                }
                                else if (!string.IsNullOrEmpty(this.Cache.Etag))
                                {
                                        service.Response.SetCache(this.Cache.Etag);
                                }
                                else
                                {
                                        service.Response.SetCache(this.Cache.ToUpdateTime, this.Cache.CacheTime);
                                }
                        }
                }


                public void WriteStream(IApiService service)
                {
                        using (Stream stream = this._Stream.Open())
                        {
                                service.Response.WriteStream(stream);
                        }
                }
        }
}
