using System.IO;

using WeDonekRpc.TcpServer.FileUp.Model;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Lock;
namespace WeDonekRpc.TcpServer.FileUp.UpStream
{
    public class SaveFileStream : ISaveStream
        {
                /// <summary>
                /// 保存的文件
                /// </summary>
                private readonly FileInfo _File = null;
                private FileStream _FileStream = null;
                private readonly LockHelper _Lock = new LockHelper();

                private volatile bool _IsSave = false;
                public string FileId { get; }

                public SaveFileStream(UpFile file) : this(file, Path.GetTempPath())
                {

                }
                public SaveFileStream(UpFile file, string dir)
                {
                        this.FileId = file.IsMd5 ? file.FileSign : string.Join("_", file.FileName, file.FileSign, file.FileSize, HeartbeatTimeHelper.CurrentDateSpan).GetMd5();
                        string path = Path.Combine(dir, string.Concat(this.FileId, Path.GetExtension(file.FileName), ".tmp"));
                        this._File = new FileInfo(path);
                }
                public void Dispose()
                {
                        if (this._FileStream != null)
                        {
                                this._FileStream.Close();
                                this._FileStream.Dispose();
                                this._FileStream = null;
                        }
                }

                public bool Lock(out IUpLockFile upLock, out string error)
                {
                        upLock = new UpLockFileStream(this._File);
                        return upLock.LoadLock(out error);
                }
                public bool CreateStream(long fileSize, out string error)
                {
                        if (!this._File.Directory.Exists)
                        {
                                this._File.Directory.Create();
                        }
                        try
                        {
                                this._FileStream = new FileStream(this._File.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1024, FileOptions.Asynchronous);
                                if (this._FileStream.Length == 0)
                                {
                                        this._FileStream.SetLength(fileSize);
                                }
                                this._File.Refresh();
                                error = null;
                                return true;
                        }
                        catch
                        {
                                error = "socket.up.file.create.error";
                                return false;
                        }
                }

                public void Write(int position, byte[] array, int offset, int count)
                {
                        if (this._Lock.GetLock())
                        {
                                this._FileStream.Position = position;
                                this._FileStream.Write(array, offset, count);
                                this._FileStream.Flush();
                                this._Lock.Exit();
                        }
                }

                public bool CheckIsExists()
                {
                        return this._File.Exists;
                }
                public void DeleteFile()
                {
                        this._File.Delete();
                }
                public void SaveStream()
                {
                        this._FileStream.Flush();
                        this.Dispose();
                        //File.Move(this._TempFilePath, this._File.FullName);
                        this._IsSave = true;
                }

                public Stream GetStream()
                {
                        if (!this._IsSave)
                        {
                                return this._FileStream;
                        }
                        return this._File.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                }

                public void Clear()
                {
                        this.Dispose();
                        if (this._File.Exists)
                        {
                                this._File.Delete();
                        }
                }

                public void Save(FileInfo file)
                {
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        this._File.MoveTo(file.FullName);
                }
        }
}
