using System;
using System.Collections.Generic;
using System.Dynamic;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Model
{
    /// <summary>
    /// 动态对象
    /// </summary>
    public class DynamicModel : DynamicObject
    {
        private readonly Type _DicType = typeof(IDictionary<string, object>);
        private readonly IDictionary<string, object> _Dic = null;
        public DynamicModel ()
        {
            this._Dic = new Dictionary<string, object>();
        }
        public DynamicModel (string json)
        {
            this._Dic = JsonTools.Json<Dictionary<string, object>>(json);
        }
        public DynamicModel (IDictionary<string, object> dic)
        {
            this._Dic = dic;
        }

        public object this[string key]
        {
            get => this._Dic[key];
            set
            {
                if (value == null)
                {
                    _ = this._Dic.Remove(key);
                }
                else if (this._Dic.ContainsKey(key))
                {
                    this._Dic[key] = value;
                }
                else
                {
                    this._Dic.Add(key, value);
                }
            }
        }

        public bool Contains (string key)
        {
            return this._Dic.ContainsKey(key);
        }
        public bool Remove (string key)
        {
            return this._Dic.Remove(key);
        }
        public string Json ()
        {
            return this._Dic.ToJson();
        }
        public override bool TryConvert (ConvertBinder binder, out object result)
        {
            if (binder.ReturnType.GetInterface(this._DicType.FullName) != null)
            {
                result = this._Dic;
                return true;
            }
            result = null;
            return false;
        }

        public override bool TryDeleteMember (DeleteMemberBinder binder)
        {
            return this._Dic.Remove(binder.Name);
        }

        public override bool TryGetMember (GetMemberBinder binder, out object result)
        {
            return this._Dic.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember (SetMemberBinder binder, object value)
        {
            if (value == null)
            {
                _ = this._Dic.Remove(binder.Name);
            }
            else if (this._Dic.ContainsKey(binder.Name))
            {
                this._Dic[binder.Name] = value;
            }
            else
            {
                this._Dic.Add(binder.Name, value);
            }
            return true;
        }
    }
}
