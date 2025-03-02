namespace WeDonekRpc.IOBuffer
{
    /// <summary>
    /// 缓冲区设置
    /// </summary>
    public class BufferSpreadSet
    {
        public BufferSpreadSet (int len, int min)
        {
            this.Len = len;
            this.Size = min;
        }
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int Len
        {
            get;
            set;
        }
        /// <summary>
        /// 缓冲区个数
        /// </summary>
        public int Size
        {
            get;
            set;
        }
    }
}
