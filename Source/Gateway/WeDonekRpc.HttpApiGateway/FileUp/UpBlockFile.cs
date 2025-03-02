using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Http;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    internal class UpBlockFile : IUpFile
    {
        private readonly UpBasicFile _FileBase;
        private readonly IBlockFile _File;
        public UpBlockFile ( UpBasicFile datum, IBlockFile file )
        {
            this._FileBase = datum;
            this._File = file;
        }

        public string ContentType => HttpHeaderHelper.GetContentType(Path.GetExtension(this._FileBase.FileName));

        public string FileName => _FileBase.FileName;

        public long FileSize => _FileBase.FileSize;

        public string FileType => "File";

        public string FileMd5 => _FileBase.FileMd5;

        public long CopyStream ( Stream stream, int offset )
        {
           return _File.CopyStream( stream, offset );
        }

        public void Dispose ()
        {

        }

        public Stream GetStream ()
        {
            return _File.GetStream();
        }

        public byte[] ReadStream ()
        {
            return _File.GetBytes();
        }

        public string SaveFile ( string savePath, bool overwrite = false )
        {
            return _File.Save(savePath, overwrite);
        }
    }
}
