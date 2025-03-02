using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcSync.Collect.linq;
using RpcSync.Collect.Model;
using RpcSync.DAL;
using RpcSync.Model;

namespace RpcSync.Collect.Collect
{
    internal class SysConfigCollect : ISysConfigCollect
    {
        private readonly ISysConfigDAL _SysConfigDAL;
        private readonly ICacheController _Cache;
        public SysConfigCollect (ISysConfigDAL configDAL,
            ICacheController cache)
        {
            this._Cache = cache;
            this._SysConfigDAL = configDAL;
        }
        public SysConfigItem GetSysConfig (string type)
        {
            string key = "SysConfig_" + ( type == string.Empty ? "public" : type );
            if (this._Cache.IsEnable && this._Cache.TryGet(key, out SysConfigItem item) && item != null)
            {
                return item;
            }
            SysConfig[] configs = this._SysConfigDAL.GetSysConfig(type).OrderByDescending(a => a.ToUpdateTime).ToArray();
            if (configs == null)
            {
                configs = new SysConfig[0];
            }
            item = new SysConfigItem
            {
                Items = configs.ConvertMap<SysConfig, ConfigItem>((a, b) =>
                {
                    b.InitConfig();
                    return b;
                }),
                Md5 = configs.GetConfigMd5()
            };
            if (this._Cache.IsEnable)
            {
                _ = this._Cache.Set(key, item, new TimeSpan(Tools.GetRandom(1, 30), 0, 0, 0));
            }
            return item;
        }

        public ConfigItemToUpdateTime[] GetToUpdateTime ()
        {
            return this._SysConfigDAL.GetToUpdateTime();
        }
        public void Refresh (string type)
        {
            string key = "SysConfig_" + ( type == string.Empty ? "public" : type );
            _ = this._Cache.Remove(key);
        }
        public Dictionary<string, SysConfigItem> GetSysConfig (string[] type)
        {
            Dictionary<string, SysConfigItem> dic = [];
            type.ForEach(c =>
            {
                dic.Add(c, this.GetSysConfig(c));
            });
            return dic;
        }
        public void Refresh (string[] type)
        {
            type.ForEach(c =>
            {
                if (c == string.Empty)
                {
                    c = "public";
                }
                string key = "SysConfig_" + c;
                _ = this._Cache.Remove(key);
            });
        }
    }
}
