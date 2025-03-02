using System;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
namespace WeDonekRpc.ModularModel.Accredit.Model
{
    public class StateParam
    {
        public StateParam ()
        {

        }

        public StateParam (object obj)
        {
            if (obj == null)
            {
                return;
            }
            Type type = obj.GetType();
            this.IsJson = Helper.HelperTools.CheckIsJson(type);
            if (this.IsJson)
            {
                this.Value = JsonTools.Json(obj);
            }
            else if (type.IsEnum)
            {
                this.Value = ( (int)obj ).ToString();
                type = PublicDataDic.IntType;
            }
            else
            {
                this.Value = obj.ToString();
            }
            this._Type = type;
            this.TypeName = type.FullName;
        }
        private Type _Type = null;

        private Type Type
        {
            get
            {
                if (this._Type == null)
                {
                    this._Type = Type.GetType(this.TypeName, false);
                }
                return this._Type;
            }
        }

        public object GetValue ()
        {
            if (this.Value == null)
            {
                return default;
            }
            else if (this.IsJson)
            {
                if (this.Type == null)
                {
                    return JsonTools.Json<object>(this.Value);
                }
                return JsonTools.Json(this.Value, this.Type);
            }
            return Helper.HelperTools.GetValue(this.Type, this.Value);
        }
        public T GetValue<T> ()
        {
            if (this.Value == null)
            {
                return default;
            }
            else if (this.IsJson)
            {
                return JsonTools.Json<T>(this.Value);
            }
            return Helper.HelperTools.GetValue<T>(this.Value);
        }
        public string Value
        {
            get;
            set;
        }

        public bool IsJson
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }
    }
}
