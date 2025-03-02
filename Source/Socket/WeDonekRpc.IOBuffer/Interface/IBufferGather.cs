namespace WeDonekRpc.IOBuffer.Interface
{
    internal interface IBuffer
    {
        ISocketBuffer ApplyBuffer ();

        void ExpandBuffer ();
        void ClearBuffer ();
    }
}