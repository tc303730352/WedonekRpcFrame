namespace SocketBuffer.Interface
{
        internal interface IBuffer
        {
                ISocketBuffer ApplyBuffer();

                void ExpandBuffer();
                void ClearBuffer();
        }
}