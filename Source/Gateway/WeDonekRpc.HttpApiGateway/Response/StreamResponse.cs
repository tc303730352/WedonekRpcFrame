using WeDonekRpc.HttpApiGateway.File;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.HttpService.Model;
using WeDonekRpc.Helper.Http;
using System.IO;

namespace WeDonekRpc.HttpApiGateway.Response
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
        public StreamResponse(FileInfo file,string name)
        {
            this._Stream = new ResponseFile(file, name);
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
        public HttpCacheSet Cache
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
                service.Response.ContentType = HttpHeaderHelper.GetContentType(this._Stream.Extension);
                if (this.Cache != null)
                {
                    service.Response.SetCache(this.Cache);
                }
            }
        }


        public void WriteStream(IApiService service)
        {
            if (this._Stream.File == null)
            {
                using (Stream stream = this._Stream.Open())
                {
                    service.Response.WriteStream(stream, this._Stream.Extension);
                }
            }
            else
            {
                service.Response.WriteFile(this._Stream.File);
            }
        }
    }
}
