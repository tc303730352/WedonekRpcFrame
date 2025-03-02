using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model;

namespace RpcSync.Service.ConfigService
{
    /// <summary>
    /// 用于替换本地配置服务数据来源
    /// </summary>
    internal class ConfigServer : IConfigServer
    {
        private ConfigItem[] _GetSysConfigItem (MsgSource source)
        {
            using (IocScope scope = RpcClient.Ioc.CreateScore())
            {
                SysConfigItem config = scope.Resolve<ISysConfigCollect>().GetSysConfig(source.SystemType);
                return config.FindItems(source, RpcServerType.后台服务);
            }
        }
        public string GetConfigVal (string name)
        {
            return null;
        }

        public T GetConfigVal<T> (string name)
        {
            return default;
        }

        public T GetConfigVal<T> (string name, T defValue)
        {
            return defValue;
        }

        public void LoadConfig ()
        {
            MsgSource source = RpcClient.CurrentSource;
            ConfigItem[] items = this._GetSysConfigItem(source);
            items.ForEach(a =>
           {
               if (a.ValueType == SysConfigValueType.JSON)
               {
                   LocalConfig.Local.SetJson(a.Name, a.Value, a.Prower);
               }
               else if (a.ValueType == SysConfigValueType.Null)
               {
                   LocalConfig.Local.SetConfig(a.Name, null, a.Prower);
               }
               else if (a.ValueType == SysConfigValueType.数字)
               {
                   LocalConfig.Local.SetValue<decimal>(a.Name, decimal.Parse(a.Value), a.Prower);
               }
               else if (a.ValueType == SysConfigValueType.Bool)
               {
                   LocalConfig.Local.SetValue<bool>(a.Name, bool.Parse(a.Value), a.Prower);
               }
               else
               {
                   LocalConfig.Local.SetConfig(a.Name, a.Value, a.Prower);
               }
           });
            using (IocScope scope = RpcClient.Ioc.CreateScore())
            {
                scope.Resolve<IServerCurConfigCollect>().Sync(source.ServerId, LocalConfig.Local.GetConfigItem());
            }
        }

        public IConfigSection GetSection (string name)
        {
            return LocalConfig.Local.GetSection(name);
        }
    }
}
