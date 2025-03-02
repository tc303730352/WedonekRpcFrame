using System;
using System.Reflection;
using System.Text;

namespace WeDonekRpc.Helper.Reflection
{
    internal class ProCache : IReflectionProperty
    {
        public bool IsObject { get; }

        public bool IsRead { get; }

        public bool IsWrite { get; }

        public string Name { get; }
        public Type Type { get; }

        private readonly PropertyGet _Get;
        private readonly PropertySet _Set;
        private readonly bool _IsAllowNull = true;
        private readonly Type _SourceType;
        public ProCache ( PropertyInfo property )
        {
            if ( property.Name == "DicLvl" )
            {

            }
            TypeCode code = Type.GetTypeCode(property.PropertyType);
            this.IsObject = code == TypeCode.Object;
            this.Type = property.PropertyType;
            this._SourceType = property.PropertyType;
            this.Name = property.Name;
            if ( this.IsObject && this.Type.Name == "Nullable`1" )
            {
                this.IsObject = false;
                this._IsAllowNull = true;
                this._SourceType = this.Type.GetGenericArguments()[0];
            }
            else
            {
                this._IsAllowNull = this.IsObject || code == TypeCode.String;
            }
            if ( property.CanRead )
            {
                MethodInfo method = property.GetMethod;
                if ( method.GetParameters().Length == 0 )
                {
                    this._Get = ReflectionTools.GetPropertyGet(method);
                    this.IsRead = true;
                }
            }
            if ( property.CanWrite )
            {
                MethodInfo method = property.SetMethod;
                if ( method.GetParameters().Length == 1 && method.ReturnType == PublicDataDic.VoidType )
                {
                    this._Set = ReflectionTools.GetPropertySet(property.SetMethod, property.PropertyType);
                    this.IsWrite = true;
                }
            }
        }

        public object GetValue ( object source )
        {
            if ( !this.IsRead )
            {
                return null;
            }
            return this._Get(source);
        }
        public void SetValue ( object source, object obj )
        {
            if ( !this.IsWrite )
            {
                throw new ErrorException("reflection.not.can.write");
            }
            else if ( obj == null )
            {
                if ( !this._IsAllowNull )
                {
                    throw new ErrorException("reflection.not.can.write.null");
                }
                this._Set(source, null);
            }
            else
            {
                if ( obj.GetType() != this._SourceType )
                {
                    obj = Tools.ChangeType(this._SourceType, obj);
                }
                this._Set(source, obj);
            }
        }

        private bool _IsEquals ( object val, object val1, Type otherType )
        {
            if ( val == null && val1 == null )
            {
                return true;
            }
            else if ( val == null || val1 == null )
            {
                return false;
            }
            else if ( this.IsObject )
            {
                return ReflectionHepler.IsEquals(this.Type, otherType, val, val1);
            }
            return val.Equals(val1);
        }
        public bool IsEquals ( IReflectionProperty otherPro, object source, object other )
        {
            object val = this.GetValue(source);
            object val1 = otherPro.GetValue(other);
            return this._IsEquals(val, val1, otherPro.Type);
        }
        public bool IsChange ( IReflectionProperty otherPro, object source, object other )
        {
            object val = this.GetValue(source);
            object val1 = otherPro.GetValue(other);
            if ( this._IsEquals(val, val1, otherPro.Type) )
            {
                return false;
            }
            this.SetValue(source, val1);
            return true;
        }
        public bool IsChange ( object source, object other )
        {
            object val = this.GetValue(source);
            object val1 = this.GetValue(other);
            if ( this._IsEquals(val, val1) )
            {
                return false;
            }
            this.SetValue(source, val1);
            return true;
        }
        private bool _IsEquals ( object val, object val1 )
        {
            if ( val == null && val1 == null )
            {
                return true;
            }
            else if ( val == null || val1 == null )
            {
                return false;
            }
            else if ( this.IsObject )
            {
                return ReflectionHepler.IsEquals(this.Type, val, val1);
            }
            return val.Equals(val1);
        }
        public bool IsEquals ( object source, object other )
        {
            object val = this.GetValue(source);
            object val1 = this.GetValue(other);
            return this._IsEquals(val, val1);
        }

        public void ToString ( object source, StringBuilder str )
        {
            object val = this.GetValue(source);
            if ( val == null )
            {
                return;
            }
            else if ( this.IsObject )
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
