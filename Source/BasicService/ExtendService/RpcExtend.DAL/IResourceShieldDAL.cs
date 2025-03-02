using RpcExtend.Model;
using WeDonekRpc.Model;

namespace RpcExtend.DAL
{
    public interface IResourceShieldDAL
    {
        ResourceShield Find (string[] keys, string path, ShieldType shieldType);
    }
}