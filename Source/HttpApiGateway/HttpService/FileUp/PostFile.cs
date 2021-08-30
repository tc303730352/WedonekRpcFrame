using System;
using System.IO;

using HttpService.Helper;

namespace HttpService
{
        public class PostFile : IUpFile
        {
                private readonly byte[] _SourceStream = null;
                internal PostFile(byte[] myByte, MultiPartInfo a)
                {
                        this._SourceStream = myByte;
                        this._FileSize = a.DataLen;
                        this._BeginIndex = a.BeginIndex;
                        this.FileName = a.FileName;
                        this.FileType = a.Name;
                        this.ContentType = a.ContentType;
                }
                private readonly int _BeginIndex = 0;
                private readonly int _FileSize = 0;
                /// <summary>
                /// 文件大小
                /// </summary>
                public long FileSize => this._FileSize;
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

                public string FileMd5 { get; private set; }

                /// <summary>
                /// 保存文件
                /// </summary>
                /// <param name="savePath"></param>
                /// <returns></returns>
                public string SaveFile(string savePath)
                {
                        string path = FileHelper.GetFileSavePath(savePath);
                        this._SaveFile(path);
                        return path;
                }

                private void _SaveFile(string path)
                {
                        FileInfo file = new FileInfo(path);
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        using (FileStream stream = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                        {
                                stream.Position = 0;
                                if (this._FileStream == null)
                                {
                                        stream.Write(this._SourceStream, this._BeginIndex, this._FileSize);
                                }
                                else
                                {
                                        stream.Write(this._FileStream, 0, this._FileSize);
                                }
                                stream.Flush();
                        }
                }
                private byte[] _FileStream = null;
                public byte[] ReadStream()
                {
                        if (this._FileStream != null)
                        {
                                return this._FileStream;
                        }
                        this._FileStream = new byte[this.FileSize];
                        Array.Copy(this._SourceStream, this._BeginIndex, this._FileStream, 0, this.FileSize);
                        return this._FileStream;
                }

                public Stream GetStream()
                {
                        byte[] myByte = this.ReadStream();
                        return new MemoryStream(myByte);
                }


                public void Dispose()
                {

                }
        }
}
