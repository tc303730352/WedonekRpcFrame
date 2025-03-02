using RpcStore.DAL.Model;
using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServiceVerConfigDAL
    {
        ServiceVerConfigModel[] Gets (long schemeId);

        SchemeSystemType[] GetSchemeType (Dictionary<long, int> systemType);
    }
}