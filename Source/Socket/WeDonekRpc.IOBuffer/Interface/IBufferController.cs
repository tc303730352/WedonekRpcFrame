namespace WeDonekRpc.IOBuffer.Interface
{
    internal interface IBufferController
    {
        /// <summary>
        /// 申请缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        ISocketBuffer ApplyBuffer (int size);
        /// <summary>
        /// 清理缓冲区
        /// </summary>
        /// <param name="time"></param>
        void ClearBuffer (int time);
        /// <summary>
        /// 扩展缓冲区
        /// </summary>
        void ExpandBuffer ();
    }
}