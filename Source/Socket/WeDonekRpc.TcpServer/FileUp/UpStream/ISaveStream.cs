using System.IO;

namespace WeDonekRpc.TcpServer.FileUp.UpStream
{
        public interface ISaveStream : System.IDisposable
        {
                string FileId { get; }

                bool Lock(out IUpLockFile upLock, out string error);

                void Write(int position, byte[] array, int offset, int count);

                bool CreateStream(long fileSize, out string error);

                bool CheckIsExists();


                void SaveStream();

                void Save(FileInfo file);
                Stream GetStream();
                void Clear();
                void DeleteFile();
        }
}