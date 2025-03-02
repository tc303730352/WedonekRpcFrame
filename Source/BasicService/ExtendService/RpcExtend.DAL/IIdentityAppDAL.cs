using RpcExtend.Model;

namespace RpcExtend.DAL
{
    public interface IIdentityAppDAL
    {
        IdentityApp GetByAppId(string appId);
    }
}