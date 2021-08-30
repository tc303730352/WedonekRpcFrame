using System.Reflection;

namespace RpcHelper.Validate
{
        internal class ValidateField : Validate
        {
                public ValidateField(FieldInfo field, IValidateAttr[] attrs) : base(field.Name, attrs)
                {
                        this._Field = field;
                        if (field.FieldType.FullName.StartsWith("System.Nullable"))
                        {
                                this._IsNullable = true;
                                base.InitValidate(field.FieldType.GenericTypeArguments[0]);
                                this._HasValue = field.FieldType.GetProperty("HasValue");
                                this._Value = field.FieldType.GetProperty("Value");
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

                private readonly FieldInfo _Field = null;
                private readonly bool _IsNullable;
                private readonly PropertyInfo _HasValue;
                private readonly PropertyInfo _Value;
                private object _GetValue(object data)
                {
                        if (!this._IsNullable)
                        {
                                return this._Field.GetValue(data);
                        }
                        object val = this._Field.GetValue(data);
                        if (val != null && (bool)this._HasValue.GetValue(val))
                        {
                                return this._Value.GetValue(val);
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
