using System;

namespace WeDonekRpc.Helper.Validate
{

    internal class ValidateCache : IValidateCache
    {
        public ValidateCache (Type type)
        {
            this.ClassType = type;
        }
        private volatile bool _IsInit = false;
        public void InitValidate ()
        {
            if (!this._IsInit)
            {
                this._IsInit = true;
                IValidateData[] attrs = DataValidateHepler.GetValidateList(this.ClassType);
                this._AttrNum = attrs.Length;
                this._AttrList = attrs;
            }
        }
        public Type ClassType
        {
            get;
        }
        public int AttrNum => this._AttrNum;

        internal IValidateData[] AttrList => this._AttrList;

        private int _AttrNum = 0;

        private IValidateData[] _AttrList = null;
        public bool ValidateData (object data, object root, out string error)
        {
            if (this._AttrNum == 0)
            {
                error = null;
                return true;
            }
            foreach (IValidateData i in this._AttrList)
            {
                if (!i.ValidateData(data, root, out error))
                {
                    return false;
                }
            }
            error = null;
            return true;
        }

        public bool CheckAttrIsValidate (Type attrType, out IValidateAttr[] validateAttrs)
        {
            if (this._AttrNum == 0)
            {
                validateAttrs = null;
                return false;
            }
            IValidateData validate = Array.Find(this._AttrList, a => a.AttrName == attrType.Name);
            if (validate == null)
            {
                validateAttrs = null;
                return false;
            }
            validateAttrs = validate.Attr;
            return true;
        }
    }
}
