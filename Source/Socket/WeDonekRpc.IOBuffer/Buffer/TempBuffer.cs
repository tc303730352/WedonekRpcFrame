namespace WeDonekRpc.IOBuffer.Buffer
{
    internal class TempBuffer : SocketBuffer
    {
        public TempBuffer (int len) : base(len, true)
        {

        }
        public override void Dispose ()
        {
        }
        public override void Dispose (int ver)
        {

        }
    }
}
