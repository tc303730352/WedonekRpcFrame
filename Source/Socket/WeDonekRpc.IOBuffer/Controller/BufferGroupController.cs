using System.Linq;

using WeDonekRpc.Helper;
using WeDonekRpc.IOBuffer.Buffer;
using WeDonekRpc.IOBuffer.Interface;

namespace WeDonekRpc.IOBuffer.Controller
{
    internal class BufferGroupController : IBufferController
    {
        private IBuffer[] _BufferList = null;

        private IBufferController _Expansion = null;
        /// <summary>
        /// 索引编号
        /// </summary>
        private int[] _Index = null;

        public BufferGroupController (BufferSpreadSet[] sets)
        {
            this._InitBuffer(sets);
        }

        public ISocketBuffer ApplyBuffer (int size)
        {
            int index = this._Index[size / 256];
            ISocketBuffer buffer = this._BufferList[index].ApplyBuffer();
            if (buffer == null)
            {
                return this._Expansion.ApplyBuffer(size);
            }
            buffer.SetBufferSize(size);
            return buffer;
        }

        public void ClearBuffer (int time)
        {
            this._BufferList.ForEach(a => a.ClearBuffer());
            this._Expansion.ClearBuffer(time);
        }

        public void ExpandBuffer ()
        {
            this._Expansion.ExpandBuffer();
            this._BufferList.ForEach(a => a.ExpandBuffer());
        }

        private void _InitBuffer (BufferSpreadSet[] sets)
        {
            int sum = sets.Sum(a => a.Size) * Config.BufferConfig.ExpansionScale / 100;
            this._Expansion = new BufferController(sum);
            int max = sets.Max(a => a.Len);
            int[] list = new int[( max / 256 ) + 1];
            int end = list.Length - 1;
            for (int i = 0; i < end; i++)
            {
                int size = ( i + 1 ) * 256;
                list[i] = sets.FindIndex(a => a.Len >= size);
            }
            list[end] = sets.FindIndex(a => a.Len >= max);
            IBuffer[] buffers = new IBuffer[sets.Length];
            this._BufferList = sets.ConvertAll((a, b) => new BufferGather(a));
            this._Index = list;
        }
    }
}
