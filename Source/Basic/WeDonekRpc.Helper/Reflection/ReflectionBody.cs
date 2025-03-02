using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Helper.Reflection
{
    public enum ReflectionType
    {
        基础类型 = 0,
        对象 = 1,
        数组 = 2,
        集合 = 3,
        字典 = 4
    }
    internal class ReflectionBody : IReflectionBody
    {
        public Type Type
        {
            get;
        }
        private readonly Type _KeyElementType;
        private readonly Type _ValueElementType;

        private readonly ReflectionType _ClassType;
        private readonly Type _ElementType;
        private readonly GetArrayValue _ArrayGet;
        private readonly SetArrayValue _ArraySet;

        private readonly GetDicValue _DicGet;
        private readonly SetDicValue _DicSet;

        private readonly Dictionary<string, IReflectionProperty> _PropertyList = [];
        public IReflectionProperty[] Properties
        {
            get;
        }

        public ReflectionType ClassType => this._ClassType;

        public ReflectionBody (Type type)
        {
            this.Type = type;
            this._ClassType = ReflectionTools.GetClassType(type);
            if (this._ClassType != ReflectionType.基础类型)
            {
                if (this._ClassType == ReflectionType.数组)
                {
                    this._ElementType = type.GetElementType();
                    MethodInfo method = type.GetMethods().Find(c =>
                    {
                        ParameterInfo[] ps = c.GetParameters();
                        return c.Name == "GetValue" && ps.Length == 1 && ps[0].ParameterType == PublicDataDic.IntType;
                    });
                    this._ArrayGet = ReflectionTools.GetArrayValue(method, type);
                    method = type.GetMethods().Find(c =>
                    {
                        ParameterInfo[] ps = c.GetParameters();
                        return c.Name == "SetValue" && ps.Length == 2 && ps[1].ParameterType == PublicDataDic.IntType;
                    });
                    this._ArraySet = ReflectionTools.SetArrayValue(method, type);
                }
                else if (this._ClassType == ReflectionType.集合)
                {
                    this._ElementType = type.GenericTypeArguments[0];
                    MethodInfo method = type.GetMethod("get_Item");
                    if (method != null)
                    {
                        this._ArrayGet = ReflectionTools.GetArrayValue(method, type);
                    }
                    method = type.GetMethod("set_Item");
                    if (method != null)
                    {
                        this._ArraySet = ReflectionTools.SetCollectionValue(method, type, this._ElementType);
                    }
                }
                else if (this._ClassType == ReflectionType.字典)
                {
                    this._KeyElementType = type.GenericTypeArguments[0];
                    this._ValueElementType = type.GenericTypeArguments[1];
                    MethodInfo method = type.GetMethod("get_Item");
                    if (method != null)
                    {
                        this._DicGet = ReflectionTools.GetDicValue(method, type, this._KeyElementType);
                    }
                    method = type.GetMethod("set_Item");
                    if (method != null)
                    {
                        this._DicSet = ReflectionTools.SetDicValue(method, type, this._KeyElementType, this._ValueElementType);
                    }
                }
                PropertyInfo[] pros = type.GetProperties();
                pros.ForEach(a =>
                {
                    if (a.IsPublic())
                    {
                        this._PropertyList.Add(a.Name, new ProCache(a));
                    }
                });
                FieldInfo[] fields = type.GetFields();
                fields.ForEach(a =>
                {
                    if (a.IsPublic)
                    {
                        this._PropertyList.Add(a.Name, new FieldCache(a));
                    }
                });
                this.Properties = this._PropertyList.Values.OrderBy(a => a.Name).ToArray();
            }
        }
        public IReflectionProperty GetProperty (string name)
        {
            if (this._PropertyList.TryGetValue(name, out IReflectionProperty property))
            {
                return property;
            }
            return null;
        }
        public object GetDicValue (object source, object key)
        {
            if (this._ClassType != ReflectionType.字典)
            {
                throw new ErrorException("public.the.operation.is.not.supported");
            }
            return this._DicGet(source, key);
        }
        public void SetDicValue (object source, object key, object obj)
        {
            if (this._ClassType != ReflectionType.字典)
            {
                throw new ErrorException("public.the.operation.is.not.supported");
            }
            this._DicSet(source, key, obj);
        }
        public object GetValue (object source, string name)
        {
            IReflectionProperty property = this.GetProperty(name);
            if (property != null && property.IsRead)
            {
                return property.GetValue(source);
            }
            return null;
        }
        public object GetValue (object source, string[] name)
        {
            object val = source;
            IReflectionBody body = this;
            if (!name.TrueForAll(a =>
            {
                IReflectionProperty property = body.GetProperty(a);
                if (property != null && property.IsRead)
                {
                    val = property.GetValue(val);
                    if (val == null)
                    {
                        return false;
                    }
                    body = ReflectionHepler.GetReflection(property.Type);
                    return true;
                }
                return false;
            }))
            {
                return null;
            }
            return val;
        }
        public bool IsEquals (IReflectionBody otherClass, object source, object other, bool? isExclude, string[] pros)
        {
            if (this._ClassType == ReflectionType.对象)
            {
                return otherClass.Properties.TrueForAll(a =>
                {
                    if (isExclude.HasValue && pros.Contains(a.Name) == isExclude.Value)
                    {
                        return true;
                    }
                    else if (this._PropertyList.TryGetValue(a.Name, out IReflectionProperty pro))
                    {
                        return pro.IsEquals(a, source, other);
                    }
                    return false;
                });
            }
            else if (this._ClassType != otherClass.ClassType || ( this._ClassType == ReflectionType.基础类型 && otherClass.Type != this.Type ))
            {
                return false;
            }
            else
            {
                return this.IsEquals(source, other, null, null);
            }
        }

        public void SetValue (object source, object obj, string name)
        {
            IReflectionProperty property = this.GetProperty(name);
            if (property != null)
            {
                property.SetValue(source, obj);
            }
            else
            {
                throw new ErrorException("reflection.property.not.find");
            }
        }
        private bool _CheckArray (object source, object other)
        {
            int num1 = (int)this.GetValue(source, "Length");
            int num2 = (int)this.GetValue(other, "Length");
            if (num1 != num2)
            {
                return false;
            }
            else if (num1 == 0)
            {
                return true;
            }
            for (int i = 0; i < num1; i++)
            {
                if (!this._ArrayContains(num1, this._ArrayGet(source, i), other))
                {
                    return false;
                }
            }
            return true;
        }
        private bool _ArrayContains (int arrayLen, object obj, object other)
        {
            for (int i = 0; i < arrayLen; i++)
            {
                if (ReflectionHepler.IsEquals(this._ElementType, obj, this._ArrayGet(other, i)))
                {
                    return true;
                }
            }
            return false;
        }
        private bool _CheckCollection (object source, object other)
        {
            ICollection sou = (ICollection)source;
            ICollection ot = (ICollection)other;
            if (sou.Count != ot.Count)
            {
                return false;
            }
            else if (sou.Count == 0)
            {
                return true;
            }
            IEnumerator list = sou.GetEnumerator();
            if (list.MoveNext())
            {
                Type type = list.Current.GetType();
                IEnumerator otList = ot.GetEnumerator();
                do
                {
                    if (!this._CollectionContains(type, list.Current, otList))
                    {
                        list.Reset();
                        return false;
                    }
                } while (list.MoveNext());
                list.Reset();
            }
            return true;
        }
        private bool _CollectionContains (Type type, object obj, IEnumerator otList)
        {
            if (otList.MoveNext())
            {
                do
                {
                    if (ReflectionHepler.IsEquals(type, obj, otList.Current))
                    {
                        otList.Reset();
                        return true;
                    }
                } while (otList.MoveNext());
                otList.Reset();
            }
            return false;
        }
        public bool IsEquals (object source, object other, bool? isExclude, string[] pros)
        {
            if (this._ClassType == ReflectionType.基础类型)
            {
                return source.Equals(other);
            }
            else if (this._ClassType == ReflectionType.对象)
            {
                return this.Properties.TrueForAll(a =>
                {
                    if (isExclude.HasValue && pros.Contains(a.Name) == isExclude.Value)
                    {
                        return true;
                    }
                    return a.IsEquals(source, other);
                });
            }
            else if (this._ClassType == ReflectionType.集合)
            {
                return this._CheckCollection(source, other);
            }
            else if (this._ClassType == ReflectionType.字典)
            {
                return this._CheckDictionary(source, other);
            }
            else
            {
                return this._CheckArray(source, other);
            }
        }
        private bool _CheckDictionary (object source, object other)
        {
            IDictionary sou = (IDictionary)source;
            IDictionary ot = (IDictionary)other;
            if (sou.Count != ot.Count)
            {
                return false;
            }
            else if (sou.Count == 0)
            {
                return true;
            }
            foreach (object i in sou.Keys)
            {
                if (!ot.Contains(i) || !ReflectionHepler.IsEquals(this._ValueElementType, sou[i], ot[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public string ToString (object source, params string[] remove)
        {
            if (this._ClassType == ReflectionType.基础类型)
            {
                return source.ToString();
            }
            StringBuilder str = new StringBuilder();
            if (this._ClassType == ReflectionType.对象)
            {
                this.Properties.ForEach(c =>
                {
                    if (!remove.IsExists(c.Name))
                    {
                        _ = str.Append(c.Name);
                        _ = str.Append("|");
                        c.ToString(source, str);
                        _ = str.Append("@");
                    }
                });
            }
            else if (this._ClassType == ReflectionType.数组)
            {
                this._WriteArray(source, str);
            }
            else
            {
                this._WriteCollection(source, str);
            }
            return str.ToString();
        }
        private void _WriteArray (object source, StringBuilder str)
        {
            int len = (int)this.GetValue(source, "Length");
            if (len == 0)
            {
                return;
            }
            for (int i = 0; i < len; i++)
            {
                ReflectionHepler.ToString(this._ElementType, this._ArrayGet(source, i), str);
                _ = str.Append("|");
            }
        }
        private void _WriteCollection (object source, StringBuilder str)
        {
            ICollection sou = (ICollection)source;
            IEnumerator list = sou.GetEnumerator();
            if (list.MoveNext())
            {
                Type type = list.Current.GetType();
                do
                {
                    ReflectionHepler.ToString(type, list.Current, str);
                    _ = str.Append("|");
                } while (list.MoveNext());
            }
        }
        public void ToString (object source, StringBuilder write)
        {
            if (this._ClassType == ReflectionType.基础类型)
            {
                _ = write.Append(source);
            }
            else if (this._ClassType == ReflectionType.对象)
            {
                this.Properties.ForEach(c =>
                {
                    _ = write.Append(c.Name);
                    _ = write.Append("[");
                    c.ToString(source, write);
                    _ = write.Append("]");
                });
            }
            else if (this._ClassType == ReflectionType.数组)
            {
                this._WriteArray(source, write);
            }
            else
            {
                this._WriteCollection(source, write);
            }
        }

        public bool TryGet (string name, out IReflectionProperty property)
        {
            return this._PropertyList.TryGetValue(name, out property);
        }

        public object GetValue (object source, int index)
        {
            if (this._ClassType == ReflectionType.基础类型)
            {
                throw new ErrorException("reflection.object.no.support");
            }
            return this._ArrayGet(source, index);
        }

        public void SetValue (object source, int index, object obj)
        {
            if (this._ClassType == ReflectionType.基础类型)
            {
                throw new ErrorException("reflection.object.no.support");
            }
            this._ArraySet(source, index, obj);
        }

        public string[] Cover (object source, object other)
        {
            return this.Properties.Convert(c =>
            {
                if (c.IsChange(source, other))
                {
                    return c.Name;
                }
                return null;
            });
        }
        public void Init (IReflectionBody sourceBody, object source, object obj)
        {
            this.Properties.ForEach(c =>
            {
                if (sourceBody.TryGet(c.Name, out IReflectionProperty pro))
                {
                    object val = pro.GetValue(source);
                    c.SetValue(obj, val);
                }
            });
        }
        public string[] Cover (IReflectionBody otherClass, object source, object other)
        {
            return this.Properties.Convert(c =>
            {
                IReflectionProperty pro = otherClass.GetProperty(c.Name);
                if (pro != null && c.IsChange(pro, source, other))
                {
                    return c.Name;
                }
                return null;
            });
        }
    }
}
