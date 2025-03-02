using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Resource;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model.Config;

namespace WeDonekRpc.Client.SystemConfig
{
    /// <summary>
    /// 远程系统配置
    /// </summary>
    public class SysRemoteConfig : IConfigServer
    {

        /// <summary>
        /// 配置集
        /// </summary>
        private readonly IConfigCollect _Config = LocalConfig.Local;

        public void LoadConfig ()
        {
            if (!ConfigRemote.GetConfig(out RemoteSysConfig config, out string error))
            {
                throw new ErrorException(error);
            }
            else if (config == null)
            {
                throw new ErrorException("rpc.get.config.fail");
            }
            else
            {
                SysConfigCollect.SetMd5(config.ConfigMd5);
                short prower = RpcStateCollect.ServerConfig.ConfigPrower;
                config.ConfigData.ForEach(a =>
                {
                    int val = a.Prower == 0 ? prower : a.Prower;
                    if (a.IsJson)
                    {
                        this._Config.SetJson(a.Name, a.Value, val);
                    }
                    else
                    {
                        this._Config.SetConfig(a.Name, a.Value, val);
                    }
                });
                ConfigRemote.SendConfig(this._Config);
            }
        }

        public string GetConfigVal (string name)
        {
            return this._Config.GetValue(name);
        }
        public T GetConfigVal<T> (string name)
        {
            return this._Config.GetValue<T>(name);
        }
        public T GetConfigVal<T> (string name, T def)
        {
            return this._Config.GetValue<T>(name, def);
        }
        public IConfigSection GetSection (string name)
        {
            return this._Config.GetSection(name);
        }
    }
}
