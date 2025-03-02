using RpcExtend.Collect.Model;
using RpcExtend.Model;

namespace RpcExtend.Collect
{
    public interface IResourceShieldCollect
    {
        ResourceShield GetShield(ShieIdQuery query, string path);
        void Refresh(string path, ShieIdQuery query);
    }
}