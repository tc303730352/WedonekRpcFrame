using RpcHelper;

namespace WebSocketGateway.Interface
{
        public interface IBatchSend
        {
                int Send(ErrorException error);
                int Send(string direct, ErrorException error);
                int Send(string direct, object result);
        }
}