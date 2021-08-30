using System;
using System.Collections;
using System.Reflection;

namespace RpcHelper.Validate
{
        internal class ValidateGeneric : IValidateAttr
        {
                public ValidateGeneric(Type type, bool isDic)
                {
                        this.SortNum = 9;
                        this.AttrName = "Generic";
                        this._IsDic = isDic;
                        this._ElementType = type;
                }
                public bool IsArray
                {
                        get;
                }
                public string ErrorMsg { get; }

                public int ErrorCode { get; }

                private readonly bool _IsDic = false;
                private readonly Type _ElementType = null;
                public int SortNum { get; }
                public string AttrName { get; }

                public bool RequiresValidationContext => true;

                private object _GetDicValue(object val)
                {
                        Type type = val.GetType();
                        PropertyInfo pro = type.GetProperty("Key");
                        if (pro != null && pro.PropertyType.IsClass && pro.PropertyType.Name == this._ElementType.Name)
                        {
                                return pro.GetValue(val);
                        }
                        pro = type.GetProperty("Value");
                        if (pro != null && pro.PropertyType.IsClass && pro.PropertyType.Name == this._ElementType.Name)
                        {
                                return pro.GetValue(val);
                        }
                        return null;
                }
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
                                                object val = em.Current;
                                                if (this._IsDic)
                                                {
                                                        val = this._GetDicValue(val);
                                                }
                                                if (val != null && !DataValidateHepler.ValidateData(this._ElementType, val, source, out error))
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
                        error = null;
                        return true;
                }

                public int CompareTo(IValidateAttr other)
                {
                        return other.SortNum.CompareTo(this.SortNum);
                }
        }
}
