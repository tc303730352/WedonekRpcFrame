using System;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateObject : IValidateAttr
    {
        internal ValidateObject()
        {
            this.AttrName = "Object";
            this.SortNum = 9;

        }
        public bool IsArray { get; }

        public string ErrorMsg { get; }

        public int ErrorCode { get; }

        public int SortNum
        {
            get;
        }

        public string AttrName
        {
            get;
        }

        public bool RequiresValidationContext => true;

        public bool CheckAttr(object source, Type type, object data, object root, out string error)
        {
            if (data == null)
            {
                error = null;
                return true;
            }
            return DataValidateHepler.ValidateData(type, data, root, out error);
        }

        public int CompareTo(IValidateAttr other)
        {
            return other.SortNum.CompareTo(this.SortNum);
        }
    }
}
