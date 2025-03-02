using RpcExtend.Model;

namespace RpcExtend.Collect
{
    public interface IIdentityAppCollect
    {
        IdentityApp GetByAppId(string id);
        void Refresh(string appId);
    }
}