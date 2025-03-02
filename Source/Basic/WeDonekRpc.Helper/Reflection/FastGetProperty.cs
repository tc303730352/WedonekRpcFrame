using System;
using System.Reflection;

namespace WeDonekRpc.Helper.Reflection
{
    internal class FastGetProperty : IFastGetProperty
    {

        public string Name { get; }

        public Type Type { get; }

        private readonly PropertyGet _Get;

        public bool IsRead { get; }

        public FastGetProperty (PropertyInfo property)
        {
            this.Type = property.PropertyType;
            this.Name = property.Name;
            if (property.CanRead)
            {
                MethodInfo method = property.GetMethod;
                if (method.GetParameters().Length == 0)
                {
                    this._Get = ReflectionTools.GetPropertyGet(method);
                    this.IsRead = true;
                }
            }
        }

        public object GetValue (object source)
        {
            return this._Get(source);
        }
    }
}
