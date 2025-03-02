using System.Text;
using System.Threading;

using WeDonekRpc.Helper;
using WeDonekRpc.IOBuffer.Interface;

namespace WeDonekRpc.IOBuffer.Buffer
{
    public class SocketBuffer : ISocketBuffer
    {
        public int SourceSize
        {
            get;
        }
        public uint PageId { get; private set; } = 0;
        public byte[] Stream { get; } = null;

        public int Ver => Interlocked.CompareExchange(ref this._ThreadId, 0, 0);

        public int BufferSize { get; private set; } = 0;

        public bool IsUsable => this._IsUsable;

        private volatile bool _IsUsable = true;
        private int _ThreadId = 0;
        public SocketBuffer (int len, bool isUsable)
        {
            this.SourceSize = len;
            this.BufferSize = len;
            this._IsUsable = isUsable;
            this.Stream = new byte[len];
        }
        public SocketBuffer (int len, int threadId) : this(len, true)
        {
            this._ThreadId = threadId;
        }

        public SocketBuffer (int len)
        {
            this.BufferSize = len;
            this.SourceSize = len;
            this.Stream = new byte[len];
        }
        public bool UsableBuffer (int threadId)
        {
            if (this._IsUsable)
            {
                return false;
            }
            this._IsUsable = true;
            return Interlocked.CompareExchange(ref this._ThreadId, threadId, 0) == 0;
        }

        public int WriteChar (char[] chars, int index)
        {
            return Encoding.UTF8.GetBytes(chars, 0, chars.Length, this.Stream, index);
        }
        public int WriteInt (int val, int index)
        {
            return BitHelper.WriteByte(val, this.Stream, index);
        }
        public int WriteUInt (uint val, int index)
        {
            return BitHelper.WriteByte(val, this.Stream, index);
        }
        public int WriteUShort (ushort val, int index)
        {
            return BitHelper.WriteByte(val, this.Stream, index);
        }
        public int WriteShort (short val, int index)
        {
            return BitHelper.WriteByte(val, this.Stream, index);
        }
        public int WriteLong (long val, int index)
        {
            return BitHelper.WriteByte(val, this.Stream, index);
        }
        public int WriteByte (byte val, int index)
        {
            this.Stream[index] = val;
            return 1;
        }
        public int Write (byte[] val, int index)
        {
            System.Buffer.BlockCopy(val, 0, this.Stream, index, val.Length);
            return val.Length;
        }
        public virtual void Dispose ()
        {
            if (this._IsUsable)
            {
                _ = Interlocked.Exchange(ref this._ThreadId, 0);
                this._IsUsable = false;
            }
        }

        public void SetBufferSize (int size, uint pageId)
        {
            this.PageId = pageId;
            this.BufferSize = size;
        }
        public void SetBufferSize (int size)
        {
            if (size == 0)
            {
                this.BufferSize = this.SourceSize;
            }
            else
            {
                this.BufferSize = size;
            }
        }

        public virtual void Dispose (int ver)
        {
            if (Interlocked.CompareExchange(ref this._ThreadId, 0, ver) == ver)
            {
                this._IsUsable = false;
            }
        }
    }
}
