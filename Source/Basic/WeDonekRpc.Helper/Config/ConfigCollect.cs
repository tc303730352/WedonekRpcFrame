using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper.Config
{
    /// <summary>
    /// 远端配置项
    /// </summary>
    internal class ConfigCollect : IConfigCollect, IConfig
    {

        /// <summary>
        /// 根配置项
        /// </summary>
        private readonly IConfigItem _Root = null;

        /// <summary>
        /// 本地配置字典
        /// </summary>
        private readonly ConcurrentDictionary<string, IConfigItem> _ConfigDic = new ConcurrentDictionary<string, IConfigItem>();
        private readonly ConcurrentDictionary<string, IConfigSection> _SectionDic = new ConcurrentDictionary<string, IConfigSection>();
        /// <summary>
        /// 配置刷新事件
        /// </summary>
        public event Action<IConfigCollect, string> RefreshEvent;

        public ConfigCollect ()
        {
            this._Root = new ConfigItem("root", this);
            _ = this._ConfigDic.TryAdd("root", this._Root);
        }
        /// <summary>
        /// 获取下级所有键
        /// </summary>
        public string[] GetKeys (string prefix)
        {
            if (prefix.IsNull())
            {
                return this._ConfigDic.Keys.ToArray();
            }
            string[] keys = this._ConfigDic.Keys.ToArray();
            int len = prefix.Length;
            return keys.Convert(c => c.StartsWith(prefix), c => c.Remove(0, len));
        }


        private void _Refresh (string name)
        {
            if (RefreshEvent != null)
            {
                _ = Task.Run(() =>
                {
                    RefreshEvent(this, name);
                });
            }
        }
        protected void _Init (string json, short prower)
        {
            JsonBodyValue dic = json.JsonObject();
            if (!dic.IsObject)
            {
                return;
            }
            else if (dic.Length == 0)
            {
                this._ConfigDic.Clear();
                return;
            }
            foreach (JsonProperty i in dic)
            {
                if (this._ConfigDic.TryGetValue(i.Name, out IConfigItem item))
                {
                    _ = item.SetValue(new JsonBodyValue(i.Value), prower);
                }
                else
                {
                    _ = this._ConfigDic.TryAdd(i.Name, new ConfigItem(i.Name, new JsonBodyValue(i.Value), this._Root, prower, this));
                }
            }
        }
        protected void _Set (string json, short prower)
        {
            JsonBodyValue val = json.JsonObject();
            if (val.IsNull || !val.IsObject)
            {
                return;
            }
            foreach (JsonProperty i in val)
            {
                this._SetValue(i.Name, new JsonBodyValue(i.Value), prower);
            }
        }
        private void _SetValue (string key, JsonBodyValue value, int prower)
        {
            if (this._ConfigDic.TryGetValue(key, out IConfigItem item))
            {
                if (item.SetValue(value, prower))
                {
                    this._Refresh(key);
                }
            }
            else if (this._ConfigDic.TryAdd(key, new ConfigItem(key, value, this._Root, prower, this)))
            {
                this._Refresh(key);
            }
        }


        public void Clear ()
        {
            this._ConfigDic.Clear();
        }
        public void SetConfig (string name, string value, int prower = 0)
        {
            if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
            {
                if (item.SetValue(value, prower))
                {
                    this._Refresh(name);
                }
            }
            else
            {
                IConfigItem add = this._CreateItem(name, ItemType.Null);
                if (this._ConfigDic.TryAdd(name, add))
                {
                    if (add.SetValue(value, ItemType.String, prower))
                    {
                        this._Refresh(name);
                    }
                }
            }
        }
        public void SetJson (string name, string json, int prower = 0)
        {
            JsonBodyValue data = json.JsonObject();
            if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
            {
                if (item.SetValue(data, prower))
                {
                    this._Refresh(name);
                }
            }
            else
            {
                IConfigItem add = this._CreateItem(name, ItemType.Object);
                if (this._ConfigDic.TryAdd(name, add))
                {
                    if (add.SetValue(data, prower))
                    {
                        this._Refresh(name);
                    }
                }
            }
        }
        public void SetValue<T> (string name, T value, int prower = 0) where T : struct
        {
            if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
            {
                if (ConfigHelper.InitObject(typeof(T), value, item, this))
                {
                    this._Refresh(name);
                }
            }
            else
            {
                IConfigItem add = this._CreateItem(name, ItemType.Null);
                if (this._ConfigDic.TryAdd(name, add))
                {
                    if (ConfigHelper.InitObject(typeof(T), value, add, this))
                    {
                        this._Refresh(name);
                    }
                }
            }
        }
        public void SetConfig<T> (string name, T value, int prower = 0) where T : class
        {
            if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
            {
                if (ConfigHelper.InitObject<T>(value, item, this))
                {
                    this._Refresh(name);
                }
            }
            else
            {
                IConfigItem p = this._CreateItem(name, ItemType.Object);
                if (ConfigHelper.InitObject<T>(value, p, this))
                {
                    this._Refresh(name);
                }
            }
        }

        private IConfigItem _CreateItem (string name, ItemType itemType)
        {
            string[] list = name.Split(':');
            if (list.Length == 1)
            {
                return new ConfigItem(name, this._Root, itemType, this);
            }
            int end = list.Length - 1;
            IConfigItem parent = this._Root;
            for (int i = 0; i < end; i++)
            {
                string key = list[i];
                if (this._ConfigDic.TryGetValue(key, out IConfigItem p))
                {
                    parent = p;
                }
                else
                {
                    parent = new ConfigItem(key, parent, ItemType.Null, this);
                    if (this._ConfigDic.TryAdd(key, parent))
                    {
                        parent.Refresh();
                    }
                }
            }
            return new ConfigItem(list.Last(), parent, itemType, this);
        }

        private string _GetValue (string name)
        {
            if (this._ConfigDic.TryGetValue(name, out IConfigItem value))
            {
                return value.ToString();
            }
            return null;
        }
        private dynamic _GetValue (string name, Type type)
        {
            string val = this._GetValue(name);
            if (string.IsNullOrEmpty(val))
            {
                return Tools.GetTypeDefValue(type);
            }
            return val.Parse(type);
        }
        #region 获取配置
        public string this[string name] => this.GetValue(name);
        public dynamic this[string name, Type type] => this._GetValue(name, type);
        public string GetValue (string name)
        {
            return this._GetValue(name);
        }
        public T GetValue<T> (string name)
        {
            string val = this.GetValue(name);
            if (string.IsNullOrEmpty(val))
            {
                return default;
            }
            return val.Parse<T>();
        }
        public T GetValue<T> (string name, T def)
        {
            string val = this._GetValue(name);
            if (string.IsNullOrEmpty(val))
            {
                return def;
            }
            return val.Parse<T>();
        }

        #endregion

        public bool SetConfig (string name, JsonBodyValue token, IConfigItem parent)
        {
            string key = string.Concat(parent.Path, ":", name);
            if (this._ConfigDic.TryGetValue(key, out IConfigItem item))
            {
                return item.SetValue(token, parent.Prower);
            }
            else if (!token.IsNull)
            {
                return this._ConfigDic.TryAdd(key, new ConfigItem(name, token, parent, this));
            }
            else
            {
                return false;
            }
        }


        public bool SetConfig (string key, Type type, object value, IConfigItem parent)
        {
            if (value == null)
            {
                return false;
            }
            string name = string.Concat(parent.Path, ":", key);
            if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
            {
                return ConfigHelper.InitObject(type, value, item, this);
            }
            else
            {
                IConfigItem add = this._CreateItem(name, ItemType.Null);
                if (this._ConfigDic.TryAdd(name, add))
                {
                    return ConfigHelper.InitObject(type, value, add, this);
                }
                return false;
            }
        }

        public string GetJson (string path)
        {
            IConfigItem[] keys = null;
            if (path == this._Root.ItemName)
            {
                keys = this._ConfigDic.Values.Where(a => a.Parent != null && a.Parent.ItemName == path).ToArray();
            }
            else
            {
                keys = this._ConfigDic.Values.Where(a => a.Parent != null && a.Parent.Path == path).ToArray();
                if (keys.Length == 0)
                {
                    return "[]";
                }
            }
            StringBuilder json = new StringBuilder("{");
            keys.ForEach(a =>
            {
                if (a.ItemType == ItemType.String)
                {
                    _ = json.AppendFormat("\"{0}\":\"{1}\",", a.ItemName, a.ToString());
                }
                else if (a.ItemType == ItemType.Object || a.ItemType == ItemType.Array)
                {
                    _ = json.AppendFormat("\"{0}\":{1},", a.ItemName, a.ToString());
                }
                else if (a.ItemType != ItemType.Null)
                {
                    _ = json.AppendFormat("\"{0}\":{1},", a.ItemName, a.ToString());
                }
            });
            _ = json.Remove(json.Length - 1, 1);
            _ = json.Append("}");
            return json.ToString();
        }

        public void SaveFile (FileInfo file)
        {
            string json = this._Root.ToString();
            Tools.WriteText(file, json, Encoding.UTF8);
        }
        public Dictionary<string, ConfigItemModel> GetConfigItem ()
        {
            return this._Root.GetConfigItem();
        }
        public IConfigSection GetSection (string name)
        {
            if (this._SectionDic.TryGetValue(name, out IConfigSection section))
            {
                return section;
            }
            section = new ConfigSection(name, this);
            if (!this._SectionDic.TryAdd(name, section))
            {
                return this.GetSection(name);
            }
            return section;
        }

        public Dictionary<string, ConfigItemModel> GetConfigItem (string path)
        {
            Dictionary<string, ConfigItemModel> items = [];
            IConfigItem[] keys = null;
            if (path == this._Root.ItemName)
            {
                keys = this._ConfigDic.Values.Where(a => a.Parent != null && a.Parent.ItemName == path).ToArray();
            }
            else
            {
                keys = this._ConfigDic.Values.Where(a => a.Parent != null && a.Parent.Path == path).ToArray();
                if (keys.Length == 0)
                {
                    return null;
                }
            }
            keys.ForEach(a =>
            {
                if (a.ItemType != ItemType.Null)
                {
                    ConfigItemModel add = new ConfigItemModel
                    {
                        ItemType = a.ItemType,
                        Prower = a.Prower
                    };
                    if (a.ItemType == ItemType.Object)
                    {
                        add.Children = a.GetObjectItem();
                    }
                    else if (a.ItemType == ItemType.Array)
                    {
                        add.ArrayItem = a.GetArrayItem();
                    }
                    else
                    {
                        add.Value = a.ToString();
                    }
                    items.Add(a.ItemName, add);
                }
            });
            if (items.Count == 0)
            {
                return null;
            }
            return items;
        }
        /// <summary>
        /// 清空所有下级
        /// </summary>
        /// <param name="itemName"></param>
        public void ClearNull (string itemName)
        {
            string key = itemName + ":";
            string[] subKeys = this._ConfigDic.Keys.Where(a => a.StartsWith(key)).ToArray();
            if (subKeys.IsNull())
            {
                return;
            }
            foreach (string i in subKeys)
            {
                _ = this._ConfigDic.Remove(i, out _);
            }
        }
        public void CoverValue (Dictionary<string, ConfigItemModel> config)
        {
            config.ForEach((a, b) =>
            {
                this.SetConfigItem(a, b);
            });
        }
        public override string ToString ()
        {
            return this._Root.ToString();
        }

        public void SetConfigItem (string name, ConfigItemModel value)
        {
            if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
            {
                item.CoverValue(value);
            }
            else
            {
                IConfigItem add = this._CreateItem(name, value.ItemType);
                if (this._ConfigDic.TryAdd(name, add))
                {
                    add.CoverValue(value);
                }
            }
        }

    }
}
