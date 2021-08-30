using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcModel;

using RpcSyncService.Controller;

using RpcHelper;
using RpcHelper.TaskTools;
namespace RpcSyncService.Collect
{
        internal class SysConfigCollect
        {
                private static readonly ConcurrentDictionary<string, SysConfigController> _SysConfigList = new ConcurrentDictionary<string, SysConfigController>();
                private const string _PublicConfigKey = "public.config";
                static SysConfigCollect()
                {
                        TaskManage.AddTask(new TaskHelper("刷新配置!", new TimeSpan(0, 1, 0), new Action(_InitConfig)));
                }
                private static void _InitConfig()
                {
                        if (_SysConfigList.Count > 0)
                        {
                                string[] keys = _SysConfigList.Keys.ToArray();
                                keys.ForEach(a =>
                                {
                                        if (_SysConfigList.TryGetValue(a, out SysConfigController config) && config.IsInit)
                                        {
                                                config.ResetLock();
                                        }
                                });
                        }
                }

                private static bool _GetSysConfig(string systemType, out SysConfigController config)
                {
                        if (!_SysConfigList.TryGetValue(systemType, out config))
                        {
                                config = _SysConfigList.GetOrAdd(systemType, new SysConfigController(systemType));
                        }
                        if (!config.Init())
                        {
                                if (_SysConfigList.TryRemove(systemType, out config))
                                {
                                        config.Dispose();
                                }
                                return false;
                        }
                        return config.IsInit;
                }

                public static string GetConfigVal(MsgSource source, string name)
                {
                        if (!_GetSysConfig(source.SystemType, out SysConfigController config))
                        {
                                return null;
                        }
                        else
                        {
                                return config.GetConfigVal(name, source);
                        }
                }

                public static RemoteSysConfig GetConfig(MsgSource source)
                {
                        if (!_GetSysConfig(source.SystemType, out SysConfigController config))
                        {
                                throw new ErrorException(config.Error);
                        }
                        else
                        {
                                return new RemoteSysConfig
                                {
                                        ConfigData = config.GetSysConfig(source),
                                        ConfigMd5 = config.ConfigMd5
                                };
                        }
                }

        }
}
