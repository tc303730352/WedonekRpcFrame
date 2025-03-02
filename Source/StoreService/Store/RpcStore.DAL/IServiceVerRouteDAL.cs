using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServiceVerRouteDAL
    {
        ServiceVerRouteModel[] Gets (long schemeId);
    }
}