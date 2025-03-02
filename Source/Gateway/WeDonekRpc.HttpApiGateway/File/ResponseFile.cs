using System.IO;

using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.File
{
    public class ResponseFile : IResponseStream
    {
        private readonly FileInfo _File = null;
        private string _FileName;
        public ResponseFile(string savePath)
        {
            this._File = new FileInfo(savePath);
            this._FileName = _File.Name;
        }
        public ResponseFile(FileInfo file)
        {
            this._File = file;
            this._FileName = file.Name;
        }
        public ResponseFile(FileInfo file,string name)
        {
            this._File = file;
            this._FileName = name;
        }

        public FileInfo File => this._File;
        public bool IsExists => this._File.Exists;

        public string Extension => _File.Extension;

        public string FileName => _FileName;

        public Stream Open()
        {
            return this._File.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

    }
}
