using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using WeDonekRpc.Helper.Format;

namespace WeDonekRpc.Helper.Validate
{
    internal class Validate : IValidateData
    {
        public Validate (string name, Type attr, IValidateAttr[] attrs)
        {
            this.AttrName = name;
            Array.Sort(attrs);
            this.Attr = attrs;
            this._AttrNum = attrs.Length;
            this.InitValidate(attr);
        }
        public Validate (string name, IValidateAttr[] attrs)
        {
            this.AttrName = name;
            Array.Sort(attrs);
            this.Attr = attrs;
            this._AttrNum = attrs.Length;
        }
        protected internal void InitValidate (Type attr)
        {
            this.AttrType = attr;
            if (attr.IsArray)
            {
                Type type = attr.GetElementType();
                if (type.IsValueType || type.Name == DataValidate.StrTypeName)
                {
                    this.IsArray = true;
                    this.ElementType = type;
                }
            }
            else if (attr.IsGenericType)
            {
                Type[] types = attr.GenericTypeArguments;
                if (types != null && types.Length == 1)
                {
                    Type type = types[0];
                    if (type.IsValueType || type.Name == DataValidate.StrTypeName)
                    {
                        this.IsGeneric = true;
                        this.ElementType = type;
                    }
                }
            }
            if (this.IsArray || this.IsGeneric)
            {
                this._IsValidateArray = Array.FindIndex(this.Attr, a => a.IsArray) != -1;
            }
        }
        private bool _IsValidateArray = true;
        public string AttrName { get; }

        private int _AttrNum = 0;

        public Type ElementType { get; private set; }
        public Type AttrType { get; private set; }
        public bool IsArray { get; private set; }

        public bool IsGeneric { get; private set; }
        public IValidateAttr[] Attr
        {
            get;
            private set;
        }
        public int AttrNum => this._AttrNum;

        public IStrFormatFilter FormatFilter
        {
            get;
            protected set;
        }

        public void SyncAttr (IValidateAttr[] attrs)
        {
            if (this.Attr.Length == 0)
            {
                Array.Sort(attrs);
                this.Attr = attrs;
                this._AttrNum = attrs.Length;
            }
            else
            {
                IValidateAttr[] t = this.Attr.FindAll(a => attrs.FindIndex(b => b.AttrName == a.AttrName) == -1);
                if (t.Length != 0)
                {
                    attrs = attrs.Concat(this.Attr).ToArray();
                }
                Array.Sort(attrs);
                this.Attr = attrs;
                this._AttrNum = attrs.Length;
            }
        }
        protected bool _ValidateData (object data, Type type, object val, bool isArray, object root, out string error)
        {
            if (this.AttrNum == 1)
            {
                return this.Attr[0].CheckAttr(data, type, val, root, out error);
            }
            else if (this.AttrNum > 0)
            {
                foreach (IValidateAttr i in this.Attr)
                {
                    if (i.IsArray == false && isArray)
                    {
                        continue;
                    }
                    else if (!i.CheckAttr(data, type, val, root, out error))
                    {
                        return false;
                    }
                    else if (i.RequiresValidationContext == false)
                    {
                        return true;
                    }
                }
            }
            error = null;
            return true;
        }
        protected bool _ValidateData (object data, object val, object root, out string error)
        {
            if (!this._IsValidateArray)
            {
                error = null;
                return true;
            }
            else if (this.IsArray && val != null)
            {
                Array array = (Array)val;
                foreach (object i in array)
                {
                    if (!this._ValidateData(data, this.ElementType, i, true, root, out error))
                    {
                        return false;
                    }
                }
            }
            else if (this.IsGeneric && val != null && this.ElementType.GetInterface("System.Collections.IEnumerable") != null)
            {
                MethodInfo methods = this.ElementType.GetMethod("GetEnumerator");
                if (methods != null)
                {
                    IEnumerator em = (IEnumerator)methods.Invoke(val, null);
                    do
                    {
                        if (em.MoveNext())
                        {
                            object i = em.Current;
                            if (!this._ValidateData(data, this.ElementType, i, true, root, out error))
                            {
                                em.Reset();
                                return false;
                            }
                        }
                        else
                        {
                            em.Reset();
                            break;
                        }
                    } while (true);
                }
            }
            error = null;
            return true;
        }
        public virtual bool ValidateData (object data, object root, out string error)
        {
            error = null;
            return true;
        }
    }
}
