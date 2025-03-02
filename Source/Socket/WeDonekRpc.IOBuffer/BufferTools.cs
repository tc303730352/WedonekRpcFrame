using System.Threading;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.IOBuffer.Buffer;
using WeDonekRpc.IOBuffer.Interface;

namespace WeDonekRpc.IOBuffer
{
    internal class BufferTools
    {

        public static int GetBufferLen (int size)
        {
            int len = size % 256;
            if (len == 0)
            {
                return size;
            }
            return ( ( size / 256 ) + 1 ) * 256;
        }
        public static ISocketBuffer UsableBuffer (ISocketBuffer[] buffer, int size)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            foreach (ISocketBuffer i in buffer)
            {
                if (i == null)
                {
                    break;
                }
                else if (i.SourceSize >= size && i.UsableBuffer(id))
                {
                    return i;
                }
            }
            return null;
        }

        public static ISocketBuffer UsableBuffer (ISocketBuffer[] buffer)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            foreach (ISocketBuffer i in buffer)
            {
                if (i.UsableBuffer(id))
                {
                    return i;
                }
            }
            return null;
        }
        private static readonly LockHelper _Lock = new LockHelper();
        internal static ISocketBuffer AddBuffer (ISocketBuffer[] list, int index, int len)
        {
            if (index < 0 || index >= list.Length)
            {
                return new TempBuffer(len);
            }
            else
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                if (_Lock.GetLock())
                {
                    if (list[index] == null)
                    {
                        ISocketBuffer buffer = new ExpansionBuffer(len, id);
                        list[index] = buffer;
                        _Lock.Exit();
                        return buffer;
                    }
                    _Lock.Exit();
                }
                return null;
            }
        }
    }

}
