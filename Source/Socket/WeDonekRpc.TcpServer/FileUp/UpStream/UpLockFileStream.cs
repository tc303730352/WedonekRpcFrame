using System.IO;

namespace WeDonekRpc.TcpServer.FileUp.UpStream
{
        internal sealed class UpLockFileStream : UpLockStream
        {
                private readonly string _LockFilePath;
                public UpLockFileStream(FileInfo file)
                {
                        this._LockFilePath = Path.Combine(file.DirectoryName, string.Concat(file.Name, ".lock"));
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                }
                protected override bool _LoadStream(out Stream stream, out string error)
                {
                        try
                        {
                                stream = new FileStream(this._LockFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 100, FileOptions.Asynchronous);
                                error = null;
                                return true;
                        }
                        catch
                        {
                                error = "socket.up.file.already.occupy";
                                stream = null;
                                return false;
                        }
                }
                public override void UnLock()
                {
                        base.UnLock();
                        if (File.Exists(this._LockFilePath))
                        {
                                File.Delete(this._LockFilePath);
                        }
                }
        }
}
