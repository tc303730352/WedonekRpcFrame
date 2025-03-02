using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.Helper;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.FileUp
{
    /// <summary>
    /// 上传文件信息
    /// </summary>
    public class UpFile : IUpFile
    {
        private readonly UpFileParam _File;
        internal UpFile ( UpFileParam param, string savePath, long size )
        {
            this.FileSize = size;
            this._File = param;
            this.TempFilePath = savePath;
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
        /// <summary>
        /// 临时文件保存路径
        /// </summary>
        internal string TempFilePath { get; } = null;

        public string FileMd5 => this._File.FileMd5;
        /// <summary>
        /// 读取流
        /// </summary>
        /// <returns></returns>
        public byte[] ReadStream ()
        {
            Stream stream = this.GetStream();
            return stream.ToBytes();
        }
        public long CopyStream ( Stream stream, int offset )
        {
            Stream file = this.GetStream();
            file.Position = 0;
            stream.Position = offset;
            file.CopyTo(stream);
            return this.FileSize;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath"></param>
        public string SaveFile ( string savePath, bool overwrite = false )
        {
            savePath = FileHelper.GetFileFullPath(savePath);
            if ( this._Stream != null )
            {
                this._Stream.Close();
                this._Stream.Dispose();
            }
            FileInfo file = new FileInfo(savePath);
            if ( file.Exists && overwrite == false )
            {
                throw new WeDonekRpc.Helper.ErrorException("http.up.file.already.exists");
            }
            else if ( !file.Directory.Exists )
            {
                file.Directory.Create();
            }
            File.Move(this.TempFilePath, savePath, overwrite);
            return savePath;
        }

        public void Dispose ()
        {
            if ( this._Stream != null )
            {
                this._Stream.Close();
                this._Stream.Dispose();
            }
            if ( File.Exists(this.TempFilePath) )
            {
                File.Delete(this.TempFilePath);
            }
        }
        private Stream _Stream = null;
        public Stream GetStream ()
        {
            if ( this._Stream == null )
            {
                this._Stream= File.Open(this.TempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            return this._Stream;
        }
    }
}
