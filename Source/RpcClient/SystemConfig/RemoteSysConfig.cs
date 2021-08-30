
using RpcClient.Interface;
using RpcClient.Resource;
using RpcClient.RpcSysEvent;

using RpcHelper;
using RpcHelper.Config;

using RpcModel;

namespace RpcClient.SystemConfig
{
        public class RemoteSysConfig : IConfigServer
        {
                /// <summary>
                /// 配置集
                /// </summary>
                private readonly IConfigCollect _Config = LocalConfig.Local;

                public RemoteSysConfig()
                {
                        RemoteSysEvent.AddEvent<SysConfigRefresh>("Rpc_RefreshConfig", this._RefreshConfig);
                }

                private string _ConfigMd5 = null;
                private TcpRemoteReply _RefreshConfig(SysConfigRefresh obj)
                {
                        if (obj.ConfigMd5 == this._ConfigMd5)
                        {
                                return null;
                        }
                        else if (!this.LoadConfig(out string error))
                        {
                                throw new ErrorException(error);
                        }
                        else
                        {
                                return null;
                        }
                }

                public bool LoadConfig(out string error)
                {
                        if (!ConfigRemote.GetConfig(out RpcModel.RemoteSysConfig config, out error))
                        {
                                return false;
                        }
                        else if (this._ConfigMd5 != config.ConfigMd5)
                        {
                                this._ConfigMd5 = config.ConfigMd5;
                                short prower = Collect.RpcStateCollect.ServerConfig.ConfigPrower;
                                config.ConfigData.ForEach(a =>
                                {
                                        if (a.IsJson)
                                        {
                                                this._Config.SetJson(a.Name, a.Value, prower);
                                        }
                                        else
                                        {
                                                this._Config.SetConfig(a.Name, a.Value, prower);
                                        }
                                });
                        }
                        return true;
                }

                public string GetConfigVal(string name)
                {
                        return this._Config.GetValue(name);
                }
                public T GetConfigVal<T>(string name)
                {
                        return this._Config.GetValue<T>(name);
                }
                public T GetConfigVal<T>(string name, T def)
                {
                        return this._Config.GetValue<T>(name, def);
                }

                public void Dispose()
                {
                        RemoteSysEvent.RemoveEvent("Rpc_RefreshConfig");
                }
        }
}
