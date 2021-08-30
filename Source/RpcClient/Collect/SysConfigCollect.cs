using System;

using RpcClient.Interface;
using RpcClient.SystemConfig;

using RpcHelper.Config;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class SysConfigCollect : ISysConfig
        {
                private static IConfigServer _ConfigServer = new RemoteSysConfig();

                private static event Action<IConfigServer, string> _RefreshEvent;

                static SysConfigCollect()
                {
                        LocalConfig.Local.RefreshEvent += _Local_RefreshEvent;
                }

                private static void _Local_RefreshEvent(IConfigCollect obj, string name)
                {
                        _Refresh(name);
                }

                public void SetLocalServer(IConfigServer server)
                {
                        _ConfigServer.Dispose();
                        _ConfigServer = new LocalSysConfig(server);
                }

                /// <summary>
                /// 注册配置刷新事件
                /// </summary>
                /// <param name="action"></param>
                public void AddRefreshEvent(Action<IConfigServer, string> action)
                {
                        _RefreshEvent += action;
                        action(_ConfigServer, string.Empty);
                }
                /// <summary>
                /// 覆盖现有配置刷新事件
                /// </summary>
                /// <param name="action"></param>
                public void SetRefreshEvent(Action<IConfigServer, string> action)
                {
                        _RefreshEvent = action;
                        action(_ConfigServer, string.Empty);
                }
                internal static bool LoadSysConfig(out string error)
                {
                        if (!_ConfigServer.LoadConfig(out error))
                        {
                                return false;
                        }
                        _Refresh(string.Empty);
                        return true;
                }
                private static void _Refresh(string name)
                {
                        if (_RefreshEvent != null)
                        {
                                _RefreshEvent(_ConfigServer, name);
                        }
                }

                public string GetConfigVal(string name)
                {
                        return _ConfigServer.GetConfigVal(name);
                }
                public T GetConfigVal<T>(string name)
                {
                        return _ConfigServer.GetConfigVal<T>(name);
                }

                public T GetConfigVal<T>(string name, T defValue)
                {
                        return _ConfigServer.GetConfigVal<T>(name, defValue);
                }
        }
}
