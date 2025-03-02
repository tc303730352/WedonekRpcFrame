using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.CurConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ServerCurConfigService : IServerCurConfigService
    {
        private readonly IServerCurConfigCollect _CurConfig;
        private readonly IDictItemCollect _DictItem;
        private readonly IDictConfig _Config;
        public ServerCurConfigService (IServerCurConfigCollect curConfig, IDictItemCollect dictItem, IDictConfig config)
        {
            this._CurConfig = curConfig;
            this._DictItem = dictItem;
            this._Config = config;
        }
        private Dictionary<string, string> _ItemName;
        private void _Load (ServerCurConfig config, ConfigItemModel item, string name)
        {
            if (item.ItemType == ItemType.Array && !item.ArrayItem.IsNull())
            {
                config.Children = item.ArrayItem.ConvertAll((c, i) =>
                {
                    ServerCurConfig add = new ServerCurConfig
                    {
                        ItemType = c.ItemType,
                        Name = "array_" + i,
                        Prower = c.Prower,
                        Value = c.Value,
                    };
                    this._Load(add, c, name);
                    return add;
                });
            }
            else if (item.Children != null && item.Children.Count > 0)
            {
                config.Children = item.Children.ConvertAll(a =>
                {
                    string key = name + ":" + a.Key;
                    ServerCurConfig add = new ServerCurConfig
                    {
                        ItemType = a.Value.ItemType,
                        Name = a.Key,
                        Prower = a.Value.Prower,
                        Value = a.Value.Value,
                        Show = this._ItemName.GetValueOrDefault(key)
                    };
                    this._Load(add, a.Value, key);
                    return add;
                });
            }
        }
        public CurConfigModel Get (long serverId)
        {
            ServerCurConfigModel config = this._CurConfig.GetConfig(serverId);
            if (config == null)
            {
                return null;
            }
            this._ItemName = this._DictItem.GetItemName(this._Config.ConfigItemShow);

            ServerCurConfig[] adds = config.CurConfig["root"].Children.ConvertAll(a =>
            {
                ServerCurConfig add = new ServerCurConfig
                {
                    ItemType = a.Value.ItemType,
                    Name = a.Key,
                    Prower = a.Value.Prower,
                    Value = a.Value.Value,
                    Show = this._ItemName.GetValueOrDefault(a.Key)
                };
                this._Load(add, a.Value, a.Key);
                return add;
            });
            return new CurConfigModel
            {
                UpTime = config.UpTime,
                Configs = adds
            };
        }
    }
}
