using System;

namespace WeDonekRpc.Helper.Validate
{
    public interface IValidateCache
    {
        int AttrNum { get; }
        Type ClassType { get; }

        void InitValidate ();
        bool ValidateData (object data, object root, out string error);

        bool CheckAttrIsValidate (Type attrType, out IValidateAttr[] attrs);
    }
}