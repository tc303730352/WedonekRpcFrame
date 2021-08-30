using RpcClient.Interface;

using RpcSyncService.Collect;

using RpcHelper;
namespace RpcSyncService.Config
{
        internal class ConfigServer : IConfigServer
        {
                public void Dispose()
                {

                }

                public string GetConfigVal(string name)
                {
                        if (!RpcClient.RpcClient.IsInit)
                        {
                                return null;
                        }
                        return SysConfigCollect.GetConfigVal(RpcClient.RpcClient.CurrentSource, name);
                }

                public T GetConfigVal<T>(string name)
                {
                        string str = this.GetConfigVal(name);
                        if (string.IsNullOrEmpty(str))
                        {
                                return default;
                        }
                        return (T)Tools.StringParse(typeof(T), str);
                }

                public T GetConfigVal<T>(string name, T defValue)
                {
                        string str = this.GetConfigVal(name);
                        if (string.IsNullOrEmpty(str))
                        {
                                return defValue;
                        }
                        return (T)Tools.StringParse(typeof(T), str);
                }

                public bool LoadConfig(out string error)
                {
                        RpcHelper.Config.LocalConfig.Local.SetConfig("rpc_config:IsEnableError", "false", short.MaxValue);
                        error = null;
                        return true;
                }
        }
}
