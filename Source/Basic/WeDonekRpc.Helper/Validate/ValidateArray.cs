using System;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateArray : IValidateAttr
    {
        public ValidateArray(Type type)
        {
            this.SortNum = 9;
            this.AttrName = "Array";
            this._ElementType = type.GetElementType();
            this.IsArray = false;
        }
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
                if (!DataValidateHepler.ValidateData(this._ElementType, i, source, out error))
                {
                    return false;
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
