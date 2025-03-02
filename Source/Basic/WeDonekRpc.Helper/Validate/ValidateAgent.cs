using System;
using System.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateAgent : Validate
    {
        private readonly Type _SourceType = null;
        public ValidateAgent(ParameterInfo param, IValidateAttr[] attrs) : base(param.Name, attrs)
        {
            this._SourceType = param.ParameterType;
            if (param.ParameterType.FullName.StartsWith("System.Nullable`1"))
            {
                this._SourceType = this._SourceType.GenericTypeArguments[0];
            }
            base.InitValidate(this._SourceType);
        }

        public override bool ValidateData(object data, object root, out string error)
        {
            if (!base._ValidateData(data, this.AttrType, data, false, root, out error))
            {
                return false;
            }
            else if (!base._ValidateData(data, data, root, out error))
            {
                return false;
            }
            return DataValidateHepler.ValidateData(this._SourceType, data, out error);
        }
    }
}
