using Wedonek.Demo.Service.Model;

namespace Wedonek.Demo.Service.Interface
{
    public interface IOrderService
    {
        long AddOrder ( OrderAddModel order );
    }
}