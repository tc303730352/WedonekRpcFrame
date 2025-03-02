using WeDonekRpc.TcpServer.FileUp.Model;

namespace WeDonekRpc.Client.FileUp.Model
{
    internal class SocketUpFile : IUpFileInfo
    {
        private UpFile _File;

        public SocketUpFile(UpFile file)
        {
            _File = file;
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName => this._File.FileName;

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize => this._File.FileSize;

        public byte[] GetByte()
        {
            return this._File.GetByte();
        }

        public string GetString()
        {
            return this._File.GetString();
        }
        public T GetData<T>()
        {
            return this._File.GetData<T>();
        }
        public T GetValue<T>() where T : struct
        {
            return this._File.GetData<T>();
        }
    }
}
