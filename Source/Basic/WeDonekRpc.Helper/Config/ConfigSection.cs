using System;

namespace WeDonekRpc.Helper.Config
{
    internal class ConfigSection : IConfigSection
    {
        private readonly IConfigCollect _Config;
        private readonly string _Name;
        private readonly string _Prefix;
        private event Action<IConfigSection, string> _Refresh;
        private bool _IsInit = false;
        public ConfigSection (string name, IConfigCollect config)
        {
            this._Name = name;
            this._Prefix = string.Concat(name, ":");
            this._Config = config;
        }

        public string ItemName => this._Name;

        public string[] Keys
        {
            get
            {
                return this._Config.GetKeys(this._Prefix);
            }
        }

        public void AddRefreshEvent (Action<IConfigSection, string> action)
        {
            _Refresh += action;
            if (!this._IsInit)
            {
                this._IsInit = true;
                this._Config.RefreshEvent += this._ConfigRefresh;
            }
            action(this, string.Empty);
        }
        public void RemoveRefreshEvent (Action<IConfigSection, string> action)
        {
            _Refresh -= action;
        }

        private void _ConfigRefresh (IConfigCollect config, string name)
        {
            if (_Refresh != null && ( name == this._Name || name.StartsWith(this._Prefix) ))
            {
                if (name != this._Name)
                {
                    name = name.Remove(0, this._Prefix.Length);
                }
                _Refresh(this, name);
            }
        }

        public T GetValue<T> ()
        {
            return this._Config.GetValue<T>(this._Name);
        }

        public T GetValue<T> (T def)
        {
            return this._Config.GetValue<T>(this._Name, def);
        }

        public string GetValue (string name)
        {
            return this._Config.GetValue(this._Prefix + name);
        }

        public T GetValue<T> (string name)
        {
            return this._Config.GetValue<T>(this._Prefix + name);
        }

        public T GetValue<T> (string name, T def)
        {
            return this._Config.GetValue<T>(this._Prefix + name, def);
        }
        public IConfigSection GetSection (string name)
        {
            return this._Config.GetSection(this._Prefix + name);
        }
    }
}
