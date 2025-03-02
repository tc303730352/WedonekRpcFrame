using System.Collections.Generic;
using WeDonekRpc.Helper;
using WeDonekRpc.IOBuffer.Interface;

namespace WeDonekRpc.IOBuffer.Buffer
{
    internal class BufferGather : IBuffer
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int BufferSize
        {
            get;
        }
        private readonly int _Size = 0;
        public BufferGather (BufferSpreadSet set)
        {
            this._Size = set.Size;
            this.BufferSize = set.Len;
            this._InitBuffer(set);
        }
        /// <summary>
        /// 缓冲区
        /// </summary>
        private ISocketBuffer[] _BufferList = null;


        private void _InitBuffer (BufferSpreadSet obj)
        {
            this._BufferList = new SocketBuffer[obj.Size];
            obj.Size.For(a =>
            {
                this._BufferList[a] = new SocketBuffer(obj.Len, false);
            });
        }
        public void ExpandBuffer ()
        {
            int num = this._BufferList.Count(a => !a.IsUsable);
            if (num == 0)
            {
                this._ExpandBuffer();
            }
        }
        public void ClearBuffer ()
        {
            if (this._BufferList.Length == this._Size)
            {
                return;
            }
            List<ISocketBuffer> usable = [];
            List<ISocketBuffer> noUsable = [];
            this._BufferList.ForEach(a =>
            {
                if (a.IsUsable)
                {
                    usable.Add(a);
                }
                else
                {
                    noUsable.Add(a);
                }
            });
            int len = this._BufferList.Length;
            int limit = len - ( len / 4 );
            if (usable.Count <= limit)
            {
                len = usable.Count;
                int add = len * 20 / 100;
                ISocketBuffer[] list = new ISocketBuffer[( len + add )];
                this._BufferList.CopyTo(list, add);
                noUsable.CopyTo(0, list, 0, add);
            }
        }
        private void _ExpandBuffer ()
        {
            int len = this._BufferList.Length;
            int add = len * 20 / 100;
            ISocketBuffer[] list = new ISocketBuffer[( len + add )];
            this._BufferList.CopyTo(list, add);
            add.For(a =>
            {
                list[a] = new SocketBuffer(this.BufferSize, false);
            });
        }
        public ISocketBuffer ApplyBuffer ()
        {
            return BufferTools.UsableBuffer(this._BufferList);
        }

    }
}
