namespace SocketBuffer
{
        public interface ISocketBuffer : System.IDisposable
        {
                /// <summary>
                /// 缓冲区大小
                /// </summary>
                int BufferSize { get; }
                /// <summary>
                /// 原大小
                /// </summary>
                int SourceSize { get; }
                /// <summary>
                /// 版本
                /// </summary>
                int Ver { get; }
                /// <summary>
                /// 是否可用
                /// </summary>
                /// <returns></returns>
                bool IsUsable { get; }
                /// <summary>
                /// 占用缓冲区
                /// </summary>
                /// <returns></returns>
                bool UsableBuffer();
                /// <summary>
                /// 包ID
                /// </summary>
                int PageId { get; }
                /// <summary>
                /// 流
                /// </summary>
                byte[] Stream { get; }
                void SetBufferSize(int size);
                void SetBufferSize(int size, int pageId);
                int Write(byte[] val, int index);
                int WriteChar(char[] chars, int index);
                int WriteInt(int val, int index);
                int WriteLong(long val, int index);
                int WriteShort(short val, int index);
                int WriteByte(byte val, int index);
                string FormatStr();

                void Dispose(int ver);
        }
}