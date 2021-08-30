using System;
using System.Collections;
using System.Reflection;

namespace RpcHelper.Validate
{
        internal class ValidateAttrGeneric : IValidateAttr
        {
                public ValidateAttrGeneric(Type type, IValidateAttr[] attr)
                {
                        this._ValidateAttr = attr;
                        this.SortNum = 9;
                        this.AttrName = "Generic";
                        this._ElementType = type;
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
                        MethodInfo methods = type.GetMethod("GetEnumerator");
                        if (methods != null)
                        {
                                IEnumerator em = (IEnumerator)methods.Invoke(data, null);
                                do
                                {
                                        if (em.MoveNext())
                                        {
                                                foreach (IValidateAttr i in this._ValidateAttr)
                                                {
                                                        if (!i.CheckAttr(source, this._ElementType, em.Current, root, out error))
                                                        {
                                                                em.Reset();
                                                                return false;
                                                        }
                                                }
                                        }
                                        else
                                        {
                                                em.Reset();
                                                break;
                                        }
                                } while (true);
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

