using System;
using System.Reflection;
using System.Text;

namespace WeDonekRpc.Helper.Reflection
{

    internal class FieldCache : IReflectionProperty
    {
        private readonly PropertyGet _Get;
        private readonly PropertySet _Set;
        private readonly Type _SourceType;
        private readonly bool _IsAllowNull;
        public bool IsObject
        {
            get;
        }
        public Type Type { get; }
        public bool IsRead { get; } = true;

        public bool IsWrite { get; } = true;

        public string Name { get; }
        public FieldCache (FieldInfo field)
        {
            this.Name = field.Name;
            this.Type = field.FieldType;
            this._SourceType = field.FieldType;
            TypeCode code = Type.GetTypeCode(field.FieldType);
            this.IsObject = code == TypeCode.Object;
            if (this.IsObject && this.Type.Name == "Nullable`1")
            {
                this.IsObject = false;
                this._IsAllowNull = true;
                this._SourceType = this.Type.GetGenericArguments()[0];
            }
            else
            {
                this._IsAllowNull = this.IsObject || code == TypeCode.String;
            }
            this._Get = ReflectionTools.GetFieldGet(field);
            this._Set = ReflectionTools.GetFieldSet(field);
        }

        public object GetValue (object source)
        {
            return this._Get(source);
        }

        public void SetValue (object source, object obj)
        {
            if (obj == null)
            {
                if (!this._IsAllowNull)
                {
                    throw new ErrorException("reflection.not.can.write.null");
                }
                this._Set(source, null);
            }
            else
            {
                if (obj.GetType() != this._SourceType)
                {
                    obj = Tools.ChangeType(this._SourceType, obj);
                }
                this._Set(source, obj);
            }
        }

        public bool IsEquals (IReflectionProperty otherPro, object source, object other)
        {
            object val = this.GetValue(source);
            object val1 = otherPro.GetValue(other);
            return this._IsEquals(val, val1, otherPro.Type);
        }
        private bool _IsEquals (object val, object val1, Type otherType)
        {
            if (val == null && val1 == null)
            {
                return true;
            }
            else if (val == null || val1 == null)
            {
                return false;
            }
            else if (this.IsObject)
            {
                return ReflectionHepler.IsEquals(this.Type, otherType, val, val1);
            }
            return val.Equals(val1);
        }
        public bool IsChange (object source, object other)
        {
            object val = this.GetValue(source);
            object val1 = this.GetValue(other);
            if (this._IsEquals(val, val1))
            {
                return false;
            }
            this.SetValue(source, val1);
            return true;
        }

        public bool IsChange (IReflectionProperty otherPro, object source, object other)
        {
            object val = this.GetValue(source);
            object val1 = otherPro.GetValue(other);
            if (this._IsEquals(val, val1, otherPro.Type))
            {
                return false;
            }
            this.SetValue(source, val1);
            return true;
        }
        private bool _IsEquals (object val, object val1)
        {
            if (val == null && val1 == null)
            {
                return true;
            }
            else if (val == null || val1 == null)
            {
                return false;
            }
            else if (this.IsObject)
            {
                return ReflectionHepler.IsEquals(this.Type, val, val1);
            }
            return val.Equals(val1);
        }
        public bool IsEquals (object source, object other)
        {
            object val = this.GetValue(source);
            object val1 = this.GetValue(other);
            return this._IsEquals(val, val1);
        }
        public void ToString (object source, StringBuilder str)
        {
            object val = this.GetValue(source);
            if (val == null)
            {
                return;
            }
            else if (this.IsObject)
            {
                ReflectionHepler.ToString(this.Type, val, str);
            }
            else
            {
                _ = str.Append(val.ToString());
            }
        }
    }
}
