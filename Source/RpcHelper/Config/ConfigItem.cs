using System.Collections.Generic;

using Newtonsoft.Json.Linq;
namespace RpcHelper.Config
{
        public enum ItemType
        {
                String = 0,
                Num = 3,
                Object = 1,
                Array = 2,
                Boolean = 4
        }
        internal class ConfigItem : IConfigItem
        {
                private string _Value = null;
                private readonly IConfig _Source = null;
                private ItemType _ItemType = ItemType.String;
                public ConfigItem(string name, IConfigItem parent, string str, short prower, IConfig source) : this(name, parent, prower, source)
                {
                        this._Value = str;
                }
                public ConfigItem(string name, IConfigItem parent, object obj, short prower, IConfig source) : this(name, parent, prower, source)
                {
                        this._ItemType = this._SetValue(obj);
                }
                public ConfigItem(JToken token, IConfigItem parent, IConfig source) : this(token.Path, parent, parent.Prower, source)
                {
                        this._ItemType = this._SetValue(token);
                }
                public ConfigItem(string name, IConfig source)
                {
                        this._Source = source;
                        this._ItemType = ItemType.Object;
                        this.Path = name;
                        this.ItemName = name;
                }
                public ConfigItem(string name, IConfigItem parent, short prower, IConfig source)
                {
                        this._Source = source;
                        this.Path = parent.Path == "root" ? name : string.Concat(parent.Path, ":", name);
                        this.Prower = prower;
                        this.Parent = parent;
                        this.ItemName = name;
                }
                public string Path
                {
                        get;
                }
                public IConfigItem Parent
                {
                        get;
                }

                public string ItemName
                {
                        get;
                }
                public short Prower
                {
                        get;
                        private set;
                }

                public ItemType ItemType => this._ItemType;

                public bool SetValue(object obj, short prower)
                {
                        if (prower < this.Prower)
                        {
                                return false;
                        }
                        this.Prower = prower;
                        this._ItemType = this._SetValue(obj);
                        this._RefreshParent();
                        return true;
                }
                private ItemType _SetValue(object obj)
                {
                        if (obj is JObject i)
                        {
                                this._SetValue(i);
                                return ItemType.Object;
                        }
                        else if (obj is JArray)
                        {
                                this._Value = obj.ToString();
                                return ItemType.Array;
                        }
                        else if (obj is JValue v)
                        {
                                this._Value = obj.ToString();
                                if (v.Type == JTokenType.Boolean)
                                {
                                        this._Value = this._Value.ToLower();
                                        return ItemType.Boolean;
                                }
                                else if (v.Type == JTokenType.Integer || v.Type == JTokenType.Float)
                                {
                                        return ItemType.Num;
                                }
                                else
                                {
                                        return ItemType.String;
                                }
                        }
                        else
                        {
                                this._Value = obj.ToString();
                                return ItemType.String;
                        }
                }
                private void _SetValue(JObject obj)
                {
                        IDictionary<string, JToken?> dic = obj;
                        foreach (string i in dic.Keys)
                        {
                                this._Source.SetConfig(dic[i], this);
                        }
                }
                public override string ToString()
                {
                        if (this._ItemType == ItemType.Object && this._Value == null)
                        {
                                this._Value = this._Source.GetJson(this.Path);
                        }
                        return this._Value;
                }

                public bool SetValue(JToken token, short prower)
                {
                        if (prower < this.Prower)
                        {
                                return false;
                        }
                        this.Prower = prower;
                        this._ItemType = this._SetValue(token);
                        this._RefreshParent();
                        return true;
                }
                private ItemType _SetValue(JToken token)
                {
                        if (token.Type == JTokenType.Object)
                        {
                                ConfigHelper.InitToken(token, this, this._Source);
                                return ItemType.Object;
                        }
                        else if (token.Type == JTokenType.Null)
                        {
                                this._Value = null;
                                return ItemType.String;
                        }
                        else if (token.Type == JTokenType.Boolean)
                        {
                                this._Value = token.ToString().ToLower();
                                return ItemType.Boolean;
                        }
                        else if (token.Type == JTokenType.Array)
                        {
                                this._Value = token.ToString();
                                return ItemType.Array;
                        }
                        else if (token.Type == JTokenType.Integer || token.Type == JTokenType.Float)
                        {
                                this._Value = token.ToString();
                                return ItemType.Num;
                        }
                        else
                        {
                                this._Value = token.ToString();
                                return ItemType.String;
                        }
                }
                private void _RefreshParent()
                {
                        if (this.Parent.ItemType == ItemType.Object && this._ItemType != ItemType.Object)
                        {
                                this.Parent.Refresh();
                        }
                }
                public bool SetValue(string obj, ItemType itemType, short prower)
                {
                        if (prower < this.Prower || this._Value == obj)
                        {
                                return false;
                        }
                        this._ItemType = itemType;
                        this.Prower = prower;
                        this._Value = obj;
                        this._RefreshParent();
                        return true;
                }
                public void Refresh()
                {
                        if (this.ItemType == ItemType.Object)
                        {
                                this._Value = null;
                                this.Parent?.Refresh();
                        }
                }
        }
}
