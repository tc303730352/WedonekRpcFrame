using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface IResourceModularDAL
    {
        long AddModular(ResourceModularModel add);
        long FindModular(string key);
    }
}