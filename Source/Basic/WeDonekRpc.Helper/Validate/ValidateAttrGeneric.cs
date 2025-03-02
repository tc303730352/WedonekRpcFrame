using System;
using System.Collections;
using WeDonekRpc.Helper.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateAttrGeneric : IValidateAttr
    {
        private readonly IValidateAttr[] _ValidateAttr = null;
        private readonly Type _ElementType = null;
        private readonly IGenericForeach _Foreach;
        public ValidateAttrGeneric(Type type, IValidateAttr[] attr)
        {
            this._ValidateAttr = attr;
            this.SortNum = 9;
            this.AttrName = "Generic";
            this._ElementType = type.GenericTypeArguments[0];
            this._Foreach = new GenericForeach(type);
            this.IsArray = false;
        }
        public bool IsArray { get; }
        public int SortNum { get; }
        public string AttrName { get; }
        public string ErrorMsg { get; }

        public bool RequiresValidationContext => true;

        public bool CheckAttr(object source, Type type, object data, object root, out string error)
        {
            if (data == null)
            {
                error = null;
                return true;
            }
            string msg = null;
            if (!_Foreach.TrueForAll(data, a => {
                foreach (IValidateAttr i in this._ValidateAttr)
                {
                    if(!i.CheckAttr(source, this._ElementType, a, root, out msg))
                    {
                        return false;
                    }
                }
                return true;
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

