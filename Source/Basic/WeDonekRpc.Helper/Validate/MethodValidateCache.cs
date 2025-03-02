using System.Collections.Generic;
using System.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class MethodValidateCache
        {
                private int _AttrNum = 0;
                private Dictionary<int, IValidateData> _AttrList;
                private volatile bool _IsInit = false;
                public MethodValidateCache(MethodInfo method)
                {
                        this.Method = method;
                }
                public void InitValidate()
                {
                        if (!this._IsInit)
                        {
                                this._IsInit = true;
                                this._AttrList = MethodValidateHelper.GetValidateList(this.Method);
                                this._AttrNum = this._AttrList.Count;
                        }
                }
                public MethodInfo Method
                {
                        get;
                }
                public int AttrNum => this._AttrNum;


                public bool ValidateData(object[] param, out string error)
                {
                        if (this._AttrNum == 0)
                        {
                                error = null;
                                return true;
                        }
                        foreach (int i in this._AttrList.Keys)
                        {
                                object val = param[i];
                                if (!this._AttrList[i].ValidateData(val, val, out error))
                                {
                                        return false;
                                }
                        }
                        error = null;
                        return true;
                }
                internal bool CheckAttrIsValidate(int index)
                {
                        if (this._AttrNum == 0)
                        {
                                return false;
                        }
                        else
                        {
                                return this._AttrList.ContainsKey(index);
                        }
                }
                internal bool CheckAttrIsValidate(int index, out IValidateAttr[] validateAttrs)
                {
                        if (this._AttrNum == 0)
                        {
                                validateAttrs = null;
                                return false;
                        }
                        else if (this._AttrList.TryGetValue(index, out IValidateData attr))
                        {
                                validateAttrs = attr.Attr;
                                return true;
                        }
                        else
                        {
                                validateAttrs = null;
                                return false;
                        }
                }
        }
}
