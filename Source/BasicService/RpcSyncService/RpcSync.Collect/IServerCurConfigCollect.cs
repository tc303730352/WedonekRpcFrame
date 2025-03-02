using WeDonekRpc.Helper.Config;

namespace RpcSync.Collect
{
    public interface IServerCurConfigCollect
    {
        void Sync (long serverId, Dictionary<string, ConfigItemModel> config);
    }
}