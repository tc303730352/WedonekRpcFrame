using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using RpcSync.DAL;
using RpcSync.Model.DB;

namespace RpcSync.Collect.Collect
{
    internal class ServerCurConfigCollect : IServerCurConfigCollect
    {
        private readonly IServerCurConfigDAL _CurConfig;

        public ServerCurConfigCollect (IServerCurConfigDAL curConfig)
        {
            this._CurConfig = curConfig;
        }

        public void Sync (long serverId, Dictionary<string, ConfigItemModel> config)
        {
            ServerCurConfigModel obj = new ServerCurConfigModel
            {
                ServerId = serverId,
                UpTime = DateTime.Now,
                CurConfig = config.ToJson()
            };
            if (this._CurConfig.IsExists(serverId))
            {
                this._CurConfig.Set(obj);
            }
            else
            {
                this._CurConfig.Add(obj);
            }
        }
    }
}
