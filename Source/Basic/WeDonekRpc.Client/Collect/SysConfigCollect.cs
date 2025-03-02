using System;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.RpcSysEvent;
using WeDonekRpc.Client.SystemConfig;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class SysConfigCollect : ISysConfig
    {
        private static IConfigServer _ConfigServer = new SystemConfig.SysRemoteConfig();

        private static event Action<IConfigServer, string> _RefreshEvent;

        static SysConfigCollect ()
        {
            RemoteSysEvent.AddEvent<SysConfigRefresh>("Rpc_RefreshConfig", _RefreshConfig);
            LocalConfig.Local.RefreshEvent += _Local_RefreshEvent;
        }

        private static string _ConfigMd5 = null;

        public IConfigServer Config => _ConfigServer;

        public static void SetMd5 (string md5)
        {
            if (_ConfigMd5 != md5)
            {
                _ConfigMd5 = md5;
            }
        }

        private static TcpRemoteReply _RefreshConfig (SysConfigRefresh obj, MsgSource source)
        {
            if (obj.ConfigMd5 == _ConfigMd5)
            {
                return null;
            }
            _ConfigMd5 = obj.ConfigMd5;
            _ConfigServer.LoadConfig();
            return null;
        }
        private static void _Local_RefreshEvent (IConfigCollect obj, string name)
        {
            _Refresh(name);
        }

        public void SetLocalServer (IConfigServer server)
        {
            _ConfigServer = new LocalSysConfig(server);
        }

        /// <summary>
        /// 注册配置刷新事件
        /// </summary>
        /// <param name="action"></param>
        public void AddRefreshEvent (Action<IConfigServer, string> action)
        {
            _RefreshEvent += action;
            action(_ConfigServer, string.Empty);
        }
        /// <summary>
        /// 覆盖现有配置刷新事件
        /// </summary>
        /// <param name="action"></param>
        public void SetRefreshEvent (Action<IConfigServer, string> action)
        {
            _RefreshEvent = action;
            action(_ConfigServer, string.Empty);
        }
        internal static void LoadSysConfig ()
        {
            _ConfigServer.LoadConfig();
            _Refresh(string.Empty);
        }
        private static void _Refresh (string name)
        {
            _RefreshEvent?.Invoke(_ConfigServer, name);
        }

        public string GetConfigVal (string name)
        {
            return _ConfigServer.GetConfigVal(name);
        }
        public T GetConfigVal<T> (string name)
        {
            return _ConfigServer.GetConfigVal<T>(name);
        }

        public T GetConfigVal<T> (string name, T defValue)
        {
            return _ConfigServer.GetConfigVal<T>(name, defValue);
        }

        public IConfigSection GetSection (string name)
        {
            return _ConfigServer.GetSection(name);
        }
    }
}
