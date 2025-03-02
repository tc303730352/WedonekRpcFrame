using System.Collections.Generic;
using System.Text.Json;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper.Config
{
    public enum ItemType
    {
        String = 0,
        Num = 3,
        Object = 1,
        Array = 2,
        Boolean = 4,
        Null = -1
    }
    internal class ConfigItem : IConfigItem
    {
        private string _Value = null;
        private readonly IConfig _Source = null;
        private ItemType _ItemType = ItemType.String;
        public ConfigItem (string name, IConfigItem parent, string str, int prower, IConfig source) : this(name, parent, prower, source)
        {
            this._Value = str;
        }

        public ConfigItem (string name, JsonBodyValue token, IConfigItem parent, int prower, IConfig source) : this(name, parent, prower, source)
        {
            _ = this._SetValue(token);
        }
        public ConfigItem (string name, JsonBodyValue token, IConfigItem parent, IConfig source) : this(name, parent, parent.Prower, source)
        {
            _ = this._SetValue(token);
        }
        public ConfigItem (string name, IConfig source)
        {
            this._Source = source;
            this._ItemType = ItemType.Object;
            this.Path = name;
            this.ItemName = name;
        }
        public ConfigItem (string name, IConfigItem parent, ItemType itemType, IConfig source) : this(name, parent, -1, source)
        {
            this._ItemType = itemType;
        }
        private ConfigItem (string name, IConfigItem parent, int prower, IConfig source)
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
        public int Prower
        {
            get;
            private set;
        }

        public ItemType ItemType => this._ItemType;

        public bool SetValue (JsonBodyValue obj, int prower)
        {
            if (prower < this.Prower)
            {
                return false;
            }
            else if (this._SetValue(obj))
            {
                this.Prower = prower;
                this._RefreshParent();
                return true;
            }
            return false;
        }
        private bool _SetValue (JsonBodyValue obj)
        {
            if (obj.IsNull)
            {
                if (this._Value == null)
                {
                    return false;
                }
                this._Value = null;
                this._ItemType = ItemType.Null;
                return true;
            }
            else if (obj.IsObject)
            {
                this._ItemType = ItemType.Object;
                return this._SetObject(obj);
            }
            else if (obj.IsArray)
            {
                string val = obj.GetJsonText();
                if (val == this._Value)
                {
                    return false;
                }
                this._Value = val;
                this._ItemType = ItemType.Array;
                return true;
            }
            JsonValueKind token = obj.ValueType;
            if (token == JsonValueKind.True)
            {
                string val = PublicDataDic.TrueValue;
                if (val == this._Value)
                {
                    return false;
                }
                this._ItemType = ItemType.Boolean;
                this._Value = val;
                return true;
            }
            else if (token == JsonValueKind.False)
            {
                string val = PublicDataDic.FalseValue;
                if (val == this._Value)
                {
                    return false;
                }
                this._ItemType = ItemType.Boolean;
                this._Value = val;
                return true;
            }
            else
            {
                string val = obj.ToString();
                if (val == this._Value)
                {
                    return false;
                }
                this._ItemType = token == JsonValueKind.Number ? ItemType.Num : ItemType.String;
                this._Value = val;
                return true;
            }
        }

        private bool _SetObject (JsonBodyValue dic)
        {
            bool isSet = false;
            foreach (JsonProperty i in dic)
            {
                if (this._Source.SetConfig(i.Name, new JsonBodyValue(i.Value), this))
                {
                    isSet = true;
                }
            }
            return isSet;
        }
        public override string ToString ()
        {
            if (this._ItemType == ItemType.Null)
            {
                return "null";
            }
            else if (this._ItemType == ItemType.Object && this._Value == null)
            {
                this._Value = this._Source.GetJson(this.Path);
            }
            return this._Value;
        }
        public Dictionary<string, ConfigItemModel> GetObjectItem ()
        {
            return this._Source.GetConfigItem(this.Path);
        }
        public Dictionary<string, ConfigItemModel> GetConfigItem ()
        {
            Dictionary<string, ConfigItemModel> items = [];
            ConfigItemModel item = new ConfigItemModel
            {
                ItemType = this._ItemType,
                Prower = this.Prower
            };
            if (this._ItemType == ItemType.Object)
            {
                item.Children = this._Source.GetConfigItem(this.Path);
                if (item.Children != null)
                {
                    items.Add(this.ItemName, item);
                }
            }
            else if (this._ItemType == ItemType.Array)
            {
                item.ArrayItem = this.GetArrayItem();
                if (item.ArrayItem != null)
                {
                    items.Add(this.ItemName, item);
                }
            }
            else
            {
                item.Value = this._Value;
                items.Add(this.ItemName, item);
            }
            if (items.Count == 0)
            {
                return null;
            }
            return items;
        }
        private Dictionary<string, ConfigItemModel> _GetObjectItem (JsonBodyValue value)
        {
            Dictionary<string, ConfigItemModel> items = new Dictionary<string, ConfigItemModel>(value.Length);
            foreach (JsonProperty i in value)
            {
                ConfigItemModel add = this._InitItem(new JsonBodyValue(i.Value));
                items.Add(i.Name, add);
            }
            return items;
        }
        private ConfigItemModel _InitItem (JsonBodyValue body)
        {
            ConfigItemModel add = new ConfigItemModel();
            if (body.IsObject)
            {
                add.ItemType = ItemType.Object;
                add.Children = this._GetObjectItem(body);
            }
            else if (body.ValueType == JsonValueKind.Array)
            {
                add.ItemType = ItemType.Array;
                add.ArrayItem = this._GetArrayItem(body);
            }
            else if (body.IsNull)
            {
                add.ItemType = ItemType.Null;
            }
            else if (body.ValueType == JsonValueKind.Number)
            {
                add.ItemType = ItemType.Num;
                add.Value = body.ToString();
            }
            else if (body.ValueType == JsonValueKind.False)
            {
                add.ItemType = ItemType.Boolean;
                add.Value = PublicDataDic.FalseValue;
            }
            else if (body.ValueType == JsonValueKind.True)
            {
                add.ItemType = ItemType.Boolean;
                add.Value = PublicDataDic.TrueValue;
            }
            else
            {
                add.ItemType = ItemType.String;
                add.Value = body.ToString();
            }
            return add;
        }
        private ConfigItemModel[] _GetArrayItem (JsonBodyValue value)
        {
            ConfigItemModel[] items = new ConfigItemModel[value.Length];
            int i = 0;
            foreach (dynamic o in value)
            {
                items[i++] = this._InitItem(new JsonBodyValue(o));
            }
            return items;
        }
        public ConfigItemModel[] GetArrayItem ()
        {
            JsonBodyValue value = this._Value.JsonObject();
            if (value.IsNull)
            {
                return null;
            }
            return this._GetArrayItem(value);
        }
        private void _RefreshParent ()
        {
            if (this.Parent != null && this.Parent.ItemType == ItemType.Object && this._ItemType != ItemType.Object)
            {
                this.Parent.Refresh();
            }
        }
        public void CoverValue (ConfigItemModel model)
        {
            this.Prower = model.Prower.HasValue ? model.Prower.Value : -1;
            if (this._ItemType == ItemType.Object)
            {
                this._Source.CoverValue(model.Children);
            }
            else
            {
                this._Value = model.Value;
            }
        }

        public bool SetValue (string value, int prower)
        {
            if (prower < this.Prower || this._Value == value)
            {
                return false;
            }
            this.Prower = prower;
            this._Value = value;
            if (value == null && this.ItemType == ItemType.Object)
            {
                this._Source.ClearNull(this.ItemName);
            }
            this._RefreshParent();
            return true;
        }

        public bool SetValue (string obj, ItemType itemType, int prower)
        {
            if (prower < this.Prower || this._Value == obj)
            {
                return false;
            }
            this.Prower = prower;
            this._Value = obj;
            this._ItemType = itemType;
            this._RefreshParent();
            return true;
        }
        public void Refresh ()
        {
            if (this.ItemType == ItemType.Object)
            {
                this._Value = null;
                this.Parent?.Refresh();
            }
        }
    }
}
