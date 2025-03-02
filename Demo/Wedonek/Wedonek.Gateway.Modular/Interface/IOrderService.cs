using Wedonek.Gateway.Modular.Model;

namespace Wedonek.Gateway.Modular.Interface
{
    public interface IOrderService
    {
        long AddOrder ( OrderParam add, long userId );
    }
}