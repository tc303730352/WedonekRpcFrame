using System.Reflection;

namespace RpcHelper.Validate
{
        internal class ValidateProperty : Validate
        {
                private readonly bool _IsNullable = false;
                private readonly PropertyInfo _HasValue = null;
                private readonly PropertyInfo _Value = null;
                public ValidateProperty(PropertyInfo property, IValidateAttr[] attrs) : base(property.Name, attrs)
                {
                        this._Property = property;
                        if (property.PropertyType.FullName.StartsWith("System.Nullable`1"))
                        {
                                this._IsNullable = true;
                                base.InitValidate(property.PropertyType.GenericTypeArguments[0]);
                                this._HasValue = property.PropertyType.GetProperty("HasValue");
                                this._Value = property.PropertyType.GetProperty("Value");
                        }
                        else
                        {
                                base.InitValidate(property.PropertyType);
                                this._InitFormatAttr(property);
                        }
                }
                private void _InitFormatAttr(PropertyInfo property)
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

                private object _GetValue(object data)
                {
                        if (!this._IsNullable)
                        {
                                return this._Property.GetValue(data);
                        }
                        object val = this._Property.GetValue(data);
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
