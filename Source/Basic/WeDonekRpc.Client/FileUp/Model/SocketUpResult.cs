using System.IO;
using WeDonekRpc.TcpServer.FileUp.Model;

namespace WeDonekRpc.Client.FileUp.Model
{
    internal class SocketUpResult : IUpResult
    {
        private readonly UpFileResult _result;
        public SocketUpResult ( UpFileResult result )
        {
            this._result = result;
        }

        /// <summary>
        /// 文件MD5
        /// </summary>
        public string FileMd5 => this._result.FileMd5;

        public void Save ( string savePath )
        {
            this._result.Save(savePath);
        }
        public byte[] GetByte ()
        {
            return this._result.GetByte();
        }

        public Stream GetStream ()
        {
            return this._result.GetStream();
        }

    }
}
