namespace WeDonekRpc.IOBuffer.Interface
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
        bool UsableBuffer (int threadId);
        /// <summary>
        /// 包ID
        /// </summary>
        uint PageId { get; }
        /// <summary>
        /// 流
        /// </summary>
        byte[] Stream { get; }
        void SetBufferSize (int size);
        void SetBufferSize (int size, uint pageId);
        int Write (byte[] val, int index);
        int WriteChar (char[] chars, int index);
        int WriteInt (int val, int index);
        int WriteUInt (uint val, int index);
        int WriteLong (long val, int index);
        int WriteUShort (ushort val, int index);
        int WriteShort (short val, int index);
        int WriteByte (byte val, int index);
        void Dispose (int ver);
    }
}