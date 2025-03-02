using System;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateAttrArray : IValidateAttr
    {
        public ValidateAttrArray(Type type, IValidateAttr[] attr)
        {
            this._ValidateAttr = attr;
            this.SortNum = 9;
            this.AttrName = "Array";
            this._ElementType = type.GetElementType();
            this.IsArray = false;
        }
        private readonly IValidateAttr[] _ValidateAttr = null;
        public bool IsArray { get; }

        private readonly Type _ElementType = null;
        public int SortNum { get; }
        public string AttrName { get; }
        public string ErrorMsg { get; }

        public int ErrorCode { get; }

        public bool RequiresValidationContext => true;

        public bool CheckAttr(object source, Type type, object data, object root, out string error)
        {
            if (data == null)
            {
                error = null;
                return true;
            }
            Array array = (Array)data;
            foreach (object i in array)
            {
                foreach (IValidateAttr k in this._ValidateAttr)
                {
                    if (!k.CheckAttr(source, this._ElementType, i, root, out error))
                    {
                        return false;
                    }
                }
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
