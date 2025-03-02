using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.FileUp.UpStream;

namespace WeDonekRpc.TcpServer.FileUp.Model
{
    public class UpFileResult
    {
        private readonly ISaveStream _Stream = null;
        private string _Md5 = null;
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string FileMd5
        {
            get
            {
                if (this._Md5 == null)
                {
                    this._Md5 = this._GetFileMd5();
                }
                return this._Md5;
            }
        }

        internal UpFileResult (ISaveStream stream)
        {
            this._Stream = stream;
        }
        private string _GetFileMd5 ()
        {
            Stream stream = this._Stream.GetStream();
            return stream.ToMd5();
        }

        public void Save (string savePath)
        {
            this._Stream.Save(new FileInfo(savePath));
        }
        public byte[] GetByte ()
        {
            Stream stream = this._Stream.GetStream();
            byte[] myByte = new byte[stream.Length];
            stream.Position = 0;
            _ = stream.Read(myByte, 0, myByte.Length);
            return myByte;
        }

        public Stream GetStream ()
        {
            return this._Stream.GetStream();
        }
    }
}
