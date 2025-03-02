using System.Text;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Response
{
    public class TextResponse : IResponse
    {
        private readonly string _Text = null;
        private readonly Encoding _Encoding;
        public TextResponse (string text)
        {
            this._Text = text;
            this._Encoding = HttpService.HttpService.Config.ResponseEncoding;
        }
        public TextResponse (string text, Encoding encoding)
        {
            this._Encoding = encoding;
            this._Text = text;
        }
        public ResponseType ResponseType => ResponseType.String;

        public virtual void InitResponse (IApiService service)
        {
            service.Response.ContentType = "text/plain";
        }

        public bool Verification (IApiService service)
        {
            return true;
        }

        public virtual void WriteStream (IApiService service)
        {
            service.Response.Write(this._Text, this._Encoding);
        }
    }
}

