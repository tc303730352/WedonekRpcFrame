using System;
using System.IO;

namespace WeDonekRpc.TcpServer.FileUp.UpStream
{
        public class SaveMemoryStream : ISaveStream
        {
                private MemoryStream _Stream = null;

                public string FileId { get; } = Guid.NewGuid().ToString("N");


                public bool CheckIsExists()
                {
                        return false;
                }
                public void DeleteFile()
                {

                }
                public bool CreateStream(long fileSize, out string error)
                {
                        this._Stream = new MemoryStream((int)fileSize);
                        error = null;
                        return true;
                }

                public void Dispose()
                {
                        if (this._Stream != null)
                        {
                                this._Stream.Dispose();
                        }
                }

                public bool Lock(out IUpLockFile upLock, out string error)
                {
                        upLock = new UpLockMemory();
                        return upLock.LoadLock(out error);
                }

                public void SaveStream()
                {
                }

                public void Write(int position, byte[] array, int offset, int count)
                {
                        this._Stream.Position = position;
                        this._Stream.Write(array, offset, count);
                }
                public Stream GetStream()
                {
                        return this._Stream;
                }
                public void Clear()
                {
                        this._Stream.Dispose();
                }

                public void Save(FileInfo file)
                {
                        using (FileStream stream = file.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                        {
                                this._Stream.WriteTo(stream);
                                stream.Flush();
                        }
                }
        }
}
