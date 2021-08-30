using System.IO;

using SocketTcpClient.UpFile.Model;

namespace SocketTcpClient.UpFile
{
        public delegate void UpFileAsync(FileUpResult result);

        public delegate void UpProgress(FileInfo file, int progress, long alreadyUpNum);
}
