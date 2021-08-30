using System.Text;
using System.Threading;

using RpcHelper;

namespace SocketBuffer.Buffer
{
        public class SocketBuffer : ISocketBuffer
        {
                public int SourceSize
                {
                        get;
                }
                public int PageId => this._PageId;
                public byte[] Stream => this._Stream;

                public int Ver => Interlocked.CompareExchange(ref this._ThreadId, 0, 0);

                public int BufferSize => this._BufferSize;

                public bool IsUsable => this._IsUsable;

                private readonly byte[] _Stream = null;

                private int _BufferSize = 0;
                private int _PageId = 0;
                private volatile bool _IsUsable = true;
                private int _ThreadId = 0;
                public SocketBuffer(int len, bool isUsable)
                {
                        this.SourceSize = len;
                        this._BufferSize = len;
                        this._IsUsable = isUsable;
                        this._Stream = new byte[len];
                }
                public SocketBuffer(int len)
                {
                        this._BufferSize = len;
                        this.SourceSize = len;
                        this._Stream = new byte[len];
                }
                public bool UsableBuffer()
                {
                        if (this._IsUsable)
                        {
                                return false;
                        }
                        this._IsUsable = true;
                        int id = Thread.CurrentThread.ManagedThreadId;
                        return Interlocked.CompareExchange(ref this._ThreadId, id, 0) == 0;
                }

                public int WriteChar(char[] chars, int index)
                {
                        return Encoding.UTF8.GetBytes(chars, 0, chars.Length, this._Stream, index);
                }
                public int WriteInt(int val, int index)
                {
                        return BitHelper.WriteByte(val, this._Stream, index);
                }
                public int WriteShort(short val, int index)
                {
                        return BitHelper.WriteByte(val, this._Stream, index);
                }
                public int WriteLong(long val, int index)
                {
                        return BitHelper.WriteByte(val, this._Stream, index);
                }
                public int WriteByte(byte val, int index)
                {
                        this._Stream[index] = val;
                        return 1;
                }
                public int Write(byte[] val, int index)
                {
                        System.Buffer.BlockCopy(val, 0, this._Stream, index, val.Length);
                        return val.Length;
                }
                public virtual void Dispose()
                {
                        if (this._IsUsable)
                        {
                                Interlocked.Exchange(ref this._ThreadId, 0);
                                this._IsUsable = false;
                        }
                }

                public void SetBufferSize(int size, int pageId)
                {
                        this._PageId = pageId;
                        this._BufferSize = size;
                }
                public void SetBufferSize(int size)
                {
                        if (size == 0)
                        {
                                this._BufferSize = this.SourceSize;
                        }
                        else
                        {
                                this._BufferSize = size;
                        }
                }

                public virtual void Dispose(int ver)
                {
                        if (Interlocked.CompareExchange(ref this._ThreadId, 0, ver) == ver)
                        {
                                this._IsUsable = false;
                        }
                }
                public string FormatStr()
                {
                        StringBuilder str = new StringBuilder(this._BufferSize);
                        for (int i = 0; i < this._BufferSize; i++)
                        {
                                str.Append(this._Stream[i].ToString());
                                str.Append(" ");
                        }
                        return str.ToString().TrimEnd();
                }
        }
}
