using System.IO;

using HttpService.Helper;

namespace HttpService
{
        /// <summary>
        /// 上传文件信息
        /// </summary>
        public class UpFile : IUpFile
        {
                internal UpFile(UpFileParam param, string savePath, long size)
                {
                        this.FileSize = size;
                        this.FileName = param.FileName;
                        this.FileMd5 = param.FileMd5;
                        this.ContentType = param.ContentType.Replace("\r\n", string.Empty);
                        this.FileType = param.Name;
                        this._TempFilePath = savePath;
                }
                private readonly string _TempFilePath = null;

                /// <summary>
                /// 文件大小
                /// </summary>
                public long FileSize { get; }
                /// <summary>
                /// 文件名
                /// </summary>
                public string FileName { get; }
                /// <summary>
                /// 文件类型
                /// </summary>
                public string FileType { get; }
                /// <summary>
                /// 内容类型
                /// </summary>
                public string ContentType { get; }
                /// <summary>
                /// 临时文件保存路径
                /// </summary>
                internal string TempFilePath => this._TempFilePath;

                public string FileMd5 { get; }

                /// <summary>
                /// 读取流
                /// </summary>
                /// <returns></returns>
                public byte[] ReadStream()
                {
                        byte[] myByte = new byte[this.FileSize];
                        Stream stream = this.GetStream();
                        stream.Position = 0;
                        stream.Read(myByte, 0, myByte.Length);
                        return myByte;
                }
                /// <summary>
                /// 保存文件
                /// </summary>
                /// <param name="savePath"></param>
                public string SaveFile(string savePath)
                {
                        savePath = FileHelper.GetFileSavePath(savePath);
                        if (this._Stream != null)
                        {
                                this._Stream.Close();
                                this._Stream.Dispose();
                        }
                        FileInfo file = new FileInfo(savePath);
                        if (file.Exists)
                        {
                                return savePath;
                        }
                        else if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        File.Move(this._TempFilePath, savePath);
                        return savePath;
                }

                public void Dispose()
                {
                        if (this._Stream != null)
                        {
                                this._Stream.Close();
                                this._Stream.Dispose();
                        }
                        if (File.Exists(this._TempFilePath))
                        {
                                File.Delete(this._TempFilePath);
                        }
                }
                private Stream _Stream = null;
                public Stream GetStream()
                {
                        if (this._Stream == null)
                        {
                                this._Stream = File.Open(this._TempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        }
                        return this._Stream;
                }
        }
}
