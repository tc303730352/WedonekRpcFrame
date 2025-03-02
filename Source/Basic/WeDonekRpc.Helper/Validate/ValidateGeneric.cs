using System;
using WeDonekRpc.Helper.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateGeneric : IValidateAttr
    {
        private readonly IGenericForeach _Foreach;
        private readonly Type _ElementType = null;
        public ValidateGeneric(Type type)
        {
            this.SortNum = 9;
            this.AttrName = "Generic";
            this._ElementType = type.GenericTypeArguments[0];
            this._Foreach = new GenericForeach(type);
        }
        public bool IsArray
        {
            get;
        }
        public string ErrorMsg { get; }

        public int SortNum { get; }

        public string AttrName { get; }

        public bool RequiresValidationContext => true;

        public bool CheckAttr(object source, Type type, object data, object root, out string error)
        {
            if (data == null)
            {
                error = null;
                return true;
            }
            string msg=null;
            if(!_Foreach.TrueForAll(data, a => {
                return !DataValidateHepler.ValidateData(this._ElementType, a, source, out msg);
            }))
            {
                error = msg;
                return false;
            }
            error = null;
            return true;
        }

        public int CompareTo(IValidateAttr other)
        {
            return other.SortNum.CompareTo(this.SortNum);
        }
    }
}
