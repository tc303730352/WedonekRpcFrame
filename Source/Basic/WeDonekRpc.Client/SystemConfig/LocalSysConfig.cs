
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.SystemConfig
{
    internal class LocalSysConfig : IConfigServer
    {
        private readonly IConfigServer _Server = null;
        private readonly IConfigCollect _Local = LocalConfig.Local;
        public LocalSysConfig (IConfigServer server)
        {
            this._Server = server;
        }

        public string GetConfigVal (string name)
        {
            string val = this._Local.GetValue(name);
            if (val == null)
            {
                return this._Server.GetConfigVal(name);
            }
            return val;
        }

        public T GetConfigVal<T> (string name)
        {
            T val = this._Local.GetValue<T>(name);
            if (val != null)
            {
                return val;
            }
            return this._Server.GetConfigVal<T>(name);
        }

        public T GetConfigVal<T> (string name, T defValue)
        {
            T val = this._Local.GetValue<T>(name);
            if (val != null && !val.Equals(default(T)))
            {
                return val;
            }
            return this._Server.GetConfigVal<T>(name, defValue);
        }

        public IConfigSection GetSection (string name)
        {
            return this._Local.GetSection(name);
        }

        public void LoadConfig ()
        {
            this._Server.LoadConfig();
        }
    }
}
