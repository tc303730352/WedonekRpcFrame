using System.Reflection;
using WeDonekRpc.Helper.Format;
using WeDonekRpc.Helper.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateField : Validate
    {
        private readonly FieldInfo _Field = null;
        private readonly bool _IsNullable;
        private readonly HasValueAction _HasValue = null;
        private readonly PropertyGet _Value = null;
        private readonly PropertyGet _Action;
        public ValidateField(FieldInfo field, IValidateAttr[] attrs) : base(field.Name, attrs)
        {
            this._Field = field;
            this._Action = ReflectionTools.GetFieldGet(field);
            if (field.FieldType.FullName.StartsWith("System.Nullable"))
            {
                this._IsNullable = true;
                base.InitValidate(field.FieldType.GenericTypeArguments[0]);
                PropertyInfo has = field.FieldType.GetProperty("HasValue");
                this._HasValue = ReflectionTools.GetNullHasValue(has.GetMethod, field.FieldType);
                PropertyInfo val = field.FieldType.GetProperty("Value");
                this._Value = ReflectionTools.GetPropertyGet(val.GetMethod);
            }
            else
            {
                base.InitValidate(field.FieldType);
            }
            this._InitFormatAttr(field);
        }
        private void _InitFormatAttr(FieldInfo field)
        {
            if (field.FieldType.Name == PublicDataDic.StringTypeName)
            {
                StrFormatFilter filter = field.GetCustomAttribute<StrFormatFilter>(true);
                if (filter != null)
                {
                    this.FormatFilter = filter;
                }
            }
        }


        private object _GetValue(object data)
        {
            if (!this._IsNullable)
            {
                return this._Action(data);
            }
            object val = this._Field.GetValue(data);
            if (val != null && this._HasValue(val))
            {
                return this._Value(val);
            }
            return null;
        }
        public override bool ValidateData(object data, object root, out string error)
        {
            object val = this._GetValue(data);
            if (val != null && this.FormatFilter != null)
            {
                val = this.FormatFilter.FormatStr(data, (string)val);
                this._Field.SetValue(data, val);
            }
            if (!base._ValidateData(data, this.AttrType, val, false, root, out error))
            {
                return false;
            }
            return base._ValidateData(data, val, root, out error);
        }
    }
}
