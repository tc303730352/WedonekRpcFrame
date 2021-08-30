using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
namespace RpcHelper.Config
{
        internal class ConfigCollect : IConfigCollect, IConfig
        {
                private readonly IConfigItem _Root = null;
                private readonly ConcurrentDictionary<string, IConfigItem> _ConfigDic = new ConcurrentDictionary<string, IConfigItem>();

                public event Action<IConfigCollect, string> RefreshEvent;

                public ConfigCollect()
                {
                        this._Root = new ConfigItem("root", this);
                        this._ConfigDic.TryAdd("root", this._Root);
                }
                private void _Refresh(string name)
                {
                        if (RefreshEvent != null)
                        {
                                Task.Run(() =>
                                {
                                        RefreshEvent(this, name);
                                });
                        }
                }
                protected void _Init(string json, short prower)
                {
                        Dictionary<string, object> dic = json.Json<Dictionary<string, object>>();
                        if (dic.Count == 0)
                        {
                                this._ConfigDic.Clear();
                                return;
                        }
                        foreach (KeyValuePair<string, object> i in dic)
                        {
                                if (this._ConfigDic.TryGetValue(i.Key, out IConfigItem item))
                                {
                                        item.SetValue(i.Value, prower);
                                }
                                else
                                {
                                        this._ConfigDic.TryAdd(i.Key, new ConfigItem(i.Key, this._Root, i.Value, prower, this));
                                }
                        }

                }
                public void Clear()
                {
                        this._ConfigDic.Clear();
                }
                public void SetConfig(string name, string value, short prower = 0)
                {
                        if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
                        {
                                if (item.SetValue(value, ItemType.String, prower))
                                {
                                        this._Refresh(name);
                                }
                        }
                        else
                        {
                                IConfigItem p = this._GetParent(name, prower);
                                if (this._ConfigDic.TryAdd(name, new ConfigItem(name, p, value, prower, this)))
                                {
                                        this._Refresh(name);
                                }
                        }
                }
                public void SetJson(string name, string json, short prower = 0)
                {
                        object data = json.Json<object>();
                        if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
                        {
                                if (item.SetValue(data, prower))
                                {
                                        this._Refresh(name);
                                }
                        }
                        else
                        {
                                IConfigItem p = this._GetParent(name, prower);
                                if (this._ConfigDic.TryAdd(name, p))
                                {
                                        if (p.SetValue(data, prower))
                                        {
                                                this._Refresh(name);
                                        }
                                }
                        }
                }
                public void SetConfig<T>(string name, T value, short prower = 0) where T : class
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
                                IConfigItem p = this._GetParent(name, prower);
                                if (ConfigHelper.InitObject<T>(value, p, this))
                                {
                                        this._Refresh(name);
                                }
                        }
                }
                private IConfigItem _Create(string name, IConfigItem parent, short prower)
                {
                        return new ConfigItem(name, parent, prower, this);
                }
                private IConfigItem _GetParent(string name, short prower)
                {
                        string[] list = name.Split(':');
                        if (list.Length == 1)
                        {
                                return this._Create(name, this._Root, prower);
                        }
                        name = list[^1];
                        for (int i = list.Length - 2; i >= 0; i--)
                        {
                                if (this._ConfigDic.TryGetValue(list[i], out IConfigItem p))
                                {
                                        return this._Create(name, p, prower);
                                }
                        }
                        return this._Create(name, this._Root, prower);
                }
                private string _GetValue(string name)
                {
                        if (this._ConfigDic.TryGetValue(name, out IConfigItem value))
                        {
                                return value.ToString();
                        }
                        return null;
                }
                private dynamic _GetValue(string name, Type type)
                {
                        string val = this._GetValue(name);
                        if (string.IsNullOrEmpty(val))
                        {
                                return Tools.GetTypeDefValue(type);
                        }
                        return Tools.StringParse(type, val);
                }
                #region 获取配置
                public string this[string name] => this.GetValue(name);
                public dynamic this[string name, Type type] => this._GetValue(name, type);
                public string GetValue(string name)
                {
                        return this._GetValue(name);
                }
                public T GetValue<T>(string name)
                {
                        string val = this.GetValue(name);
                        if (string.IsNullOrEmpty(val))
                        {
                                return default;
                        }
                        return (T)Tools.StringParse(typeof(T), val);
                }
                public T GetValue<T>(string name, T def)
                {
                        string val = this._GetValue(name);
                        if (string.IsNullOrEmpty(val))
                        {
                                return def;
                        }
                        return (T)Tools.StringParse(typeof(T), val);
                }
                #endregion

                public bool SetConfig(JToken token, IConfigItem parent)
                {
                        string key = string.Concat(parent.Path, ":", token.Path);
                        if (this._ConfigDic.TryGetValue(key, out IConfigItem item))
                        {
                                return item.SetValue(token, parent.Prower);
                        }
                        else
                        {
                                return this._ConfigDic.TryAdd(key, new ConfigItem(token, parent, this));
                        }
                }
                public bool SetConfig(string name, object val, IConfigItem parent)
                {
                        string key = string.Concat(parent.Path, ":", name);
                        if (this._ConfigDic.TryGetValue(key, out IConfigItem item))
                        {
                                return item.SetValue(val, parent.Prower);
                        }
                        else
                        {
                                return this._ConfigDic.TryAdd(key, new ConfigItem(name, parent, val, parent.Prower, this));
                        }
                }

                public bool SetConfig(string key, Type type, object value, IConfigItem parent)
                {
                        string name = string.Concat(parent.Path, ":", key);
                        if (this._ConfigDic.TryGetValue(name, out IConfigItem item))
                        {
                                return ConfigHelper.InitObject(type, value, item, this);
                        }
                        else
                        {
                                IConfigItem p = this._GetParent(name, parent.Prower);
                                if (this._ConfigDic.TryAdd(name, p))
                                {
                                        return ConfigHelper.InitObject(type, value, p, this);
                                }
                                return false;
                        }
                }
                public string GetJson(string path)
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
                                        return string.Empty;
                                }
                        }
                        StringBuilder json = new StringBuilder("{");
                        keys.ForEach(a =>
                        {
                                if (a.ItemType == ItemType.String)
                                {
                                        json.AppendFormat("\"{0}\":\"{1}\",", a.ItemName, a.ToString());
                                }
                                else
                                {
                                        json.AppendFormat("\"{0}\":{1},", a.ItemName, a.ToString());
                                }
                        });
                        json.Remove(json.Length - 1, 1);
                        json.Append("}");
                        return json.ToString();
                }
        }
}
