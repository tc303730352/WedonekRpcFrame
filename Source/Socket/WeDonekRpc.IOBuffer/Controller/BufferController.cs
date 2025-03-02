using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.IOBuffer.Buffer;
using WeDonekRpc.IOBuffer.Interface;

namespace WeDonekRpc.IOBuffer.Controller
{
    internal class BufferController : IBufferController
    {
        public BufferController (int size)
        {
            this._SourceSize = size;
            this._BufferList = new ExpansionBuffer[size];
        }
        /// <summary>
        /// 缓冲区
        /// </summary>
        private volatile ExpansionBuffer[] _BufferList = null;


        private readonly int _SourceSize = 0;

        /// <summary>
        /// 当前缓冲区索引位
        /// </summary>
        private int _BufferIndex = -1;

        private ISocketBuffer _CreateBuffer (int size)
        {
            int len = BufferTools.GetBufferLen(size);
            int index = Interlocked.Increment(ref this._BufferIndex);
            return BufferTools.AddBuffer(this._BufferList, index, len);
        }
        public ISocketBuffer ApplyBuffer (int size)
        {
            ISocketBuffer buffer = BufferTools.UsableBuffer(this._BufferList, size);
            if (buffer == null)
            {
                buffer= this._CreateBuffer(size);
                if(buffer == null)
                {
                    return this.ApplyBuffer(size);
                }
            }
            buffer.SetBufferSize(size);
            return buffer;
        }
        public void ClearBuffer (int time)
        {
            int len = Interlocked.CompareExchange(ref this._BufferIndex, 0, 0);
            if (len == -1)
            {
                return;
            }
            int size = this._BufferList.Length;
            ExpansionBuffer[] buffers = this._BufferList.FindAll(a => a != null && !a.CheckIsOverTime(time));
            if (buffers.Length == size)
            {
                return;
            }
            int newSize = buffers.Length + ( buffers.Length * 10 / 100 );
            if (newSize < this._SourceSize)
            {
                newSize = this._SourceSize;
            }
            ExpansionBuffer[] datas = new ExpansionBuffer[newSize];
            if (buffers.Length > 0)
            {
                buffers.CopyTo(datas, 0);
            }
            this._BufferList = datas;
            _ = Interlocked.Exchange(ref this._BufferIndex, buffers.Length - 1);
        }

        public void ExpandBuffer ()
        {
            int len = Interlocked.CompareExchange(ref this._BufferIndex, 0, 0);
            if (len < this._BufferList.Length)
            {
                return;
            }
            int size = len + ( len * 10 / 100 );
            int index = this._BufferList.Length - 1;
            ExpansionBuffer[] datas = new ExpansionBuffer[size];
            this._BufferList.CopyTo(datas, 0);
            this._BufferList = datas;
            _ = Interlocked.Exchange(ref this._BufferIndex, index);
        }
    }
}
