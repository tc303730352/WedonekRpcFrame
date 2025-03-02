using System;
using System.IO;
using System.Text;
using System.Threading;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpApiGateway.FileUp.Model
{
    internal class UpFileForm
    {
        private static readonly byte _Zero = 0;

        private static readonly byte _One = 1;
        /// <summary>
        /// 文件大小
        /// </summary>
        public long fileSize;
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string fileMd5;
        /// <summary>
        /// 块大小
        /// </summary>
        public int blockSize;
        /// <summary>
        /// 已上传大小
        /// </summary>
        public long alreadyUp;

        private byte[] state;

        /// <summary>
        /// 后缀名
        /// </summary>
        public string extension;

        public void Create ( FileStream stream )
        {
            this._InitState();
            this.Write(stream);
        }
        public bool CheckIsUp ( int index )
        {
            return this.state[index] == _One;
        }
        public int[] GetUpIndex ( int take )
        {
            return this.state.FindAllIndex(take, a => a == _Zero);
        }
        public void Reset ( FileStream stream )
        {
            this.alreadyUp = 0;
            this.state.Initialize();
            this.Write(stream);
        }
        private void _InitState ()
        {
            int len = (int)( this.fileSize / this.blockSize );
            if ( this.fileSize % this.blockSize != 0 )
            {
                len += 1;
            }
            this.state = new byte[len];
        }
        public byte[] ToByte ()
        {
            int len = 52 + this.state.Length;
            byte[] bytes = new byte[len];
            _ = BitHelper.WriteByte(this.fileSize, bytes, 0);
            _ = BitHelper.WriteByte(this.blockSize, bytes, 8);
            _ = BitHelper.WriteByte(this.alreadyUp, bytes, 12);
            _ = Encoding.UTF8.GetBytes(this.fileMd5, 0, 32, bytes, 20);
            Buffer.BlockCopy(this.state, 0, bytes, 52, this.state.Length);
            return bytes;
        }
        public void Write ( FileStream stream )
        {
            byte[] datas = this.ToByte();
            stream.Position = 0;
            stream.Write(datas, 0, datas.Length);
            stream.Flush();
        }
        public void Init ( FileStream stream )
        {
            stream.Position = 0;
            byte[] bytes = new byte[52];
            _ = stream.Read(bytes, 0, bytes.Length);
            this.fileSize = BitConverter.ToInt64(bytes);
            this.blockSize = BitConverter.ToInt32(bytes, 8);
            this.alreadyUp = BitConverter.ToInt64(bytes, 12);
            this.fileMd5 = Encoding.UTF8.GetString(bytes, 20, 32);
            this._InitState();
            _ = stream.Read(this.state, 0, this.state.Length);
        }
        public bool CheckIsComplate ()
        {
            return this.fileSize == this.alreadyUp;

        }
        public bool? WriteAlreadyUp ( FileStream stream, int index, int size )
        {
            if ( this.state[index] == _One )
            {
                return null;
            }
            this.state[index] = _One;
            long total = Interlocked.Add(ref this.alreadyUp, size);
            byte[] val = BitConverter.GetBytes(total);
            stream.Position = 12;
            stream.Write(val, 0, 8);
            stream.Position = 52 + index;
            stream.WriteByte(_One);
            stream.Flush();
            if ( total > this.fileSize )
            {
                throw new ErrorException("gateway.http.up.file.error");
            }
            return total == this.fileSize;
        }

    }
}
