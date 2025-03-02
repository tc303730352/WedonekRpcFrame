using System.IO;

using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.File
{
    internal class ResponseStream : IResponseStream
    {
        private readonly Stream _Stream = null;
        public ResponseStream(Stream stream, string fileName)
        {
            this._Stream = stream;
            this.FileName = fileName;
            this.Extension = Path.GetExtension(fileName);
        }
        public ResponseStream(byte[] stream, string fileName)
        {
            this._Stream = new MemoryStream(stream);
            this.FileName = fileName;
            this.Extension = Path.GetExtension(fileName);
        }
        public string FileName { get; }
        public FileInfo File { get; }
        public bool IsExists { get; } = true;
        public string Extension { get; }



        public Stream Open()
        {
            return this._Stream;
        }
    }
}
