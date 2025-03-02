using System;
using System.Collections;
using System.Reflection;
using WeDonekRpc.Helper.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateDictionary : IValidateAttr
    {
        private readonly PropertyGet _KeyGet;
        private readonly Type _KeyType;
        private readonly PropertyGet _ValueGet;
        private readonly Type _ValueType;
        private readonly GetDictionaryEnumerator _Action;
        private readonly int _range;
        public ValidateDictionary (Type type, int range)
        {
            this.SortNum = 9;
            this._range = range;
            this.AttrName = "Dictionary";
            MethodInfo method = type.GetMethod("GetEnumerator");
            this._KeyType = type.GenericTypeArguments[0];
            this._ValueType = type.GenericTypeArguments[1];
            this._Action = ReflectionTools.GetDictionaryEnumerator(method, type);
            Type keyValue = type.GetInterfaces().Find(a => a.FullName.StartsWith("System.Collections.Generic.ICollection`1"), a => a.GenericTypeArguments[0]);
            PropertyInfo pro = keyValue.GetProperty("Key");
            this._KeyGet = ReflectionTools.GetPropertyGet(pro.GetMethod);
            pro = keyValue.GetProperty("Value");
            this._ValueGet = ReflectionTools.GetPropertyGet(pro.GetMethod);
        }
        public bool IsArray
        {
            get;
        }
        public string ErrorMsg { get; }


        public int SortNum { get; }
        public string AttrName { get; }

        public bool RequiresValidationContext => true;

        public bool CheckAttr (object source, Type type, object data, object root, out string error)
        {
            if (data == null)
            {
                error = null;
                return true;
            }
            IDictionaryEnumerator em = this._Action(data);
            do
            {
                if (em.MoveNext())
                {
                    object val = em.Current;
                    if (this._range == 6 || this._range == 2)
                    {
                        object key = this._KeyGet(val);
                        if (!DataValidateHepler.ValidateData(this._KeyType, key, source, out error))
                        {
                            em.Reset();
                            return false;
                        }
                    }
                    if (this._range == 4 || this._range == 6)
                    {
                        object value = this._ValueGet(val);
                        if (!DataValidateHepler.ValidateData(this._ValueType, value, source, out error))
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
            error = null;
            return true;
        }

        public int CompareTo (IValidateAttr other)
        {
            return other.SortNum.CompareTo(this.SortNum);
        }
    }
}

