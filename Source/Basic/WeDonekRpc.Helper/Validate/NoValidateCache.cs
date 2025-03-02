using System;

namespace WeDonekRpc.Helper.Validate
{
    internal class NoValidateCache : IValidateCache
    {
        public NoValidateCache (Type type)
        {
            this.ClassType = type;
        }

        public int AttrNum { get; }

        public Type ClassType { get; }

        public bool CheckAttrIsValidate (Type attrType, out IValidateAttr[] attrs)
        {
            attrs = null;
            return false;
        }

        public void InitValidate ()
        {

        }

        public bool ValidateData (object data, object root, out string error)
        {
            error = null;
            return false;
        }
    }
}
