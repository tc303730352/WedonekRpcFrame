using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Response
{
    public sealed class XmlResponse : IResponse
    {
        private readonly string _Xml = null;
        public XmlResponse(string xml)
        {
            this._Xml = xml;
        }

        public void InitResponse(IApiService service)
        {
            service.Response.ContentType = "application/xml";
        }

        public bool Verification(IApiService service)
        {
            return true;
        }

        public void WriteStream(IApiService service)
        {
            service.Response.Write(this._Xml);
        }
    }
}
