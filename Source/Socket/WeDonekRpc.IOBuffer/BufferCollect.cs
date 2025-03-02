using System.Linq;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.IOBuffer.Buffer;
using WeDonekRpc.IOBuffer.Controller;
using WeDonekRpc.IOBuffer.Interface;
namespace WeDonekRpc.IOBuffer
{
    public class BufferCollect
    {
        private static readonly Timer _Clear = new Timer(_ClearBuffer, null, 120000, 120000);
        private static readonly Timer _Timer = new Timer(_ExpandBuffer, null, 60200, 60200);
        /// <summary>
        /// 缓冲区
        /// </summary>
        private static volatile IBufferController _Buffer = null;

        /// <summary>
        /// 扩容区(缓冲区*0.4)
        /// </summary>
        private static volatile IBufferController _Expansion = null;

        /// <summary>
        /// 最大缓冲Buffer大小
        /// </summary>
        private static int _MaxBufferSize = 0;
        private static volatile bool _IsInit = false;
        public static void InitBuffer ()
        {
            if (_IsInit)
            {
                return;
            }
            _IsInit = true;
            _InitBuffer();
        }
        private static void _ClearBuffer (object state)
        {
            int time = HeartbeatTimeHelper.HeartbeatTime - 60;
            _Expansion.ClearBuffer(time);
            _Buffer.ClearBuffer(time);

        }
        private static void _ExpandBuffer (object state)
        {
            _Expansion.ExpandBuffer();
            _Buffer.ExpandBuffer();
        }
        private static void _InitBuffer ()
        {
            BufferSpreadSet[] sets = Config.BufferConfig.BufferSpread;
            _MaxBufferSize = sets.Max(a => a.Len);
            _Buffer = new BufferGroupController(sets);
            _Expansion = new BufferController(Config.BufferConfig.ExpansionBufferSize);
        }


        public static ISocketBuffer ApplySendBuffer (int size, uint pageId)
        {
            ISocketBuffer buffer = ApplyBuffer(size);
            buffer.SetBufferSize(size, pageId);
            return buffer;
        }
        public static ISocketBuffer ApplyBuffer (int size)
        {
            if (size > Config.BufferConfig.MaxBufferSize)
            {
                return new TempBuffer(size);
            }
            else
            {
                return size > _MaxBufferSize ? _Expansion.ApplyBuffer(size) : _Buffer.ApplyBuffer(size);
            }
        }
        public static ISocketBuffer ApplyBuffer (int size, ref int ver)
        {
            ISocketBuffer buffer = ApplyBuffer(size);
            ver = buffer.Ver;
            return buffer;
        }

        public static ISocketBuffer ApplyBuffer (int size, ISocketBuffer buffer, ref int min, ref int ver)
        {
            if (buffer.SourceSize >= size && ( buffer.SourceSize <= min || size >= min ))
            {
                buffer.SetBufferSize(size);
                return buffer;
            }
            else
            {
                buffer.Dispose(ver);
                return ApplyBuffer(size, ref ver);
            }
        }
    }
}
