namespace SocketTcpServer.Interface
{
        public interface ISocketClient
        {
                bool ReplyMsg<T>(T msg, out string error);
        }
}
