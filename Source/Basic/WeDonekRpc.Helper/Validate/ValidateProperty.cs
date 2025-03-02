using System.Reflection;
using WeDonekRpc.Helper.Format;
using WeDonekRpc.Helper.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateProperty : Validate
    {
        private readonly bool _IsNullable = false;
        private readonly HasValueAction _HasValue = null;
        private readonly PropertyGet _Value = null;
        private readonly PropertyGet _Action;
        public ValidateProperty (PropertyInfo property, IValidateAttr[] attrs) : base(property.Name, attrs)
        {
            this._Property = property;
            this._Action = ReflectionTools.GetPropertyGet(property.GetMethod);
            if (property.PropertyType.Name.StartsWith("System.Nullable`1"))
            {
                this._IsNullable = true;
                base.InitValidate(property.PropertyType.GenericTypeArguments[0]);
                PropertyInfo has = property.PropertyType.GetProperty("HasValue");
                this._HasValue = ReflectionTools.GetNullHasValue(has.GetMethod, property.PropertyType);
                PropertyInfo val = property.PropertyType.GetProperty("Value");
                this._Value = ReflectionTools.GetPropertyGet(val.GetMethod);
            }
            else
            {
                base.InitValidate(property.PropertyType);
                this._InitFormatAttr(property);
            }
        }
        private void _InitFormatAttr (PropertyInfo property)
        {
            if (property.PropertyType.Name == PublicDataDic.StringTypeName)
            {
                StrFormatFilter filter = property.GetCustomAttribute<StrFormatFilter>(true);
                if (filter != null)
                {
                    this.FormatFilter = filter;
                }
            }
        }
        private readonly PropertyInfo _Property = null;

        private object _GetValue (object data)
        {
            if (data == null)
            {
                return null;
            }
            else if (!this._IsNullable)
            {
                return this._Action(data);
            }
            object val = this._Property.GetValue(data);
            if (val != null && this._HasValue(val))
            {
                return this._Value(val);
            }
            return null;
        }

        public override bool ValidateData (object data, object root, out string error)
        {
            object val = this._GetValue(data);
            if (val != null && this.FormatFilter != null)
            {
                val = this.FormatFilter.FormatStr(data, (string)val);
                this._Property.SetValue(data, val);
            }
            if (!base._ValidateData(data, this.AttrType, val, false, root, out error))
            {
                return false;
            }
            return base._ValidateData(data, val, root, out error);
        }

    }
}
