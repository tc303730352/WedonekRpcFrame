using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.Helper;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.FileUp
{
    public class PostFile : IUpFile
    {
        private readonly Stream _Stream = null;
        private readonly UpFileParam _File;
        internal PostFile ( UpFileParam param, Stream stream )
        {
            this._File = param;
            this._Stream = stream;
            this.FileSize = stream.Length;
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName => this._File.FileName;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType => this._File.Name;
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType => this._File.ContentType;

        public string FileMd5 => this._File.FileMd5;

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public string SaveFile ( string savePath, bool overwrite = false )
        {
            string path = FileHelper.GetFileFullPath(savePath);
            this._SaveFile(path, overwrite);
            return path;
        }

        private void _SaveFile ( string path, bool overwrite )
        {
            FileInfo file = new FileInfo(path);
            if ( !file.Directory.Exists )
            {
                file.Directory.Create();
            }
            else if ( file.Exists && !overwrite )
            {
                throw new ErrorException("http.up.file.already.exists");
            }
            using ( FileStream stream = file.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Read) )
            {
                stream.Position = 0;
                this._Stream.CopyTo(stream);
                stream.Flush();
            }
        }
        public byte[] ReadStream ()
        {
            return _Stream.ToBytes();
        }
        public long CopyStream ( Stream stream, int offset )
        {
            stream.Position = offset;
            this._Stream.Position = 0;
            this._Stream.CopyTo(stream);
            return this._Stream.Length;
        }

        public Stream GetStream ()
        {
            this._Stream.Position = 0;
            return this._Stream;
        }


        public void Dispose ()
        {
            this._Stream.Dispose();
        }
    }
}
