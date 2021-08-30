using System.IO;

using HttpApiGateway.Interface;

namespace HttpApiGateway.File
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

                public string Extension { get; }
                public bool IsExists { get; } = true;
                public string FileName { get; }



                public Stream Open()
                {
                        return this._Stream;
                }
        }
}
