using System.IO;

using HttpApiGateway.Interface;

namespace HttpApiGateway.File
{
        public class ResponseFile : IResponseStream
        {
                private readonly FileInfo _File = null;

                public ResponseFile(string savePath)
                {
                        this._File = new FileInfo(savePath);
                }
                public ResponseFile(FileInfo file)
                {
                        this._File = file;
                }

                public string Extension => this._File.Extension;
                public bool IsExists => this._File.Exists;

                public string FileName => this._File.Name;

                public Stream Open()
                {
                        return this._File.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                }

        }
}
