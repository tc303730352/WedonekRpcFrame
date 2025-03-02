using Wedonek.Gateway.WebSocket.Model;

namespace Wedonek.Gateway.WebSocket.Interface
{
    public interface IOrderService
    {
        long AddOrder ( OrderParam add, long userId );
    }
}