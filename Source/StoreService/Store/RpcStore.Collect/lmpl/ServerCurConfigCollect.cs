using RpcStore.DAL;
using RpcStore.Model.DB;

namespace RpcStore.Collect.lmpl
{
    internal class ServerCurConfigCollect : IServerCurConfigCollect
    {
        private readonly IServerCurConfigDAL _CurConfig;

        public ServerCurConfigCollect (IServerCurConfigDAL curConfig)
        {
            this._CurConfig = curConfig;
        }

        public ServerCurConfigModel GetConfig (long serverId)
        {
            return this._CurConfig.GetConfig(serverId);
        }
    }
}
