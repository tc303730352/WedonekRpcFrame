using System;
using System.Reflection;
using WeDonekRpc.Helper.Format;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateParameter : Validate
        {
                private readonly ParameterInfo _Param = null;
                public ValidateParameter(ParameterInfo param, IValidateAttr[] attrs) : base(param.Name, attrs)
                {
                        this._Param = param;
                        Type type = param.ParameterType;
                        if (param.ParameterType.FullName.StartsWith("System.Nullable`1"))
                        {
                                base.InitValidate(type.GenericTypeArguments[0]);
                        }
                        else
                        {
                                base.InitValidate(type);
                                this._InitFormatAttr(type);
                        }
                }
                private void _InitFormatAttr(Type type)
                {
                        if (type.Name == PublicDataDic.StringTypeName)
                        {
                                StrFormatFilter filter = this._Param.GetCustomAttribute<StrFormatFilter>(true);
                                if (filter != null)
                                {
                                        this.FormatFilter = filter;
                                }
                        }
                }


                public override bool ValidateData(object data, object root, out string error)
                {
                        if (data != null && this.FormatFilter != null)
                        {
                                data = this.FormatFilter.FormatStr(data, (string)data);
                                object[] param = (object[])root;
                                param[this._Param.Position] = data;
                        }
                        if (!base._ValidateData(data, this.AttrType, data, false, root, out error))
                        {
                                return false;
                        }
                        return base._ValidateData(data, data, root, out error);
                }

        }
}
