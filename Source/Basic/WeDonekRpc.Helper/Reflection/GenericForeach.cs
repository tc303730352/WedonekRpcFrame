using System;
using System.Collections.Generic;
using System.Reflection;

namespace WeDonekRpc.Helper.Reflection
{
    internal class GenericForeach : IGenericForeach
    {
        private readonly GetEnumerator _Action;
        private readonly PropertyGet _Current;
        private readonly Reset _Reset;
        private readonly MoveNext _Next;
        private readonly Type _Type;
        private readonly Type _ElementType;
        private readonly bool _IsObject;
        public GenericForeach ( Type type )
        {
            this._Type = type;
            MethodInfo method = type.GetMethod("GetEnumerator");
            if ( method == null )
            {
                return;
            }
            this.IsForeach = true;
            this._ElementType = type.IsArray ? type.GetElementType() : type.GenericTypeArguments[0];
            this._IsObject = Type.GetTypeCode(this._ElementType) == TypeCode.Object;
            Type enType = method.ReturnType;
            this._Action = ReflectionTools.GetEnumerator(method, type);
            this._Next = ReflectionTools.GetEnumeratorMoveNext(enType.GetMethod("MoveNext"), enType);
            this._Current = ReflectionTools.GetPropertyGet(enType.GetProperty("Current").GetMethod);
            method = enType.GetMethod("Reset");
            if ( method == null )
            {
                this._Reset = ReflectionTools.GetEnumeratorReset(enType.GetMethod("Dispose"), enType);
                return;
            }
            this._Reset = ReflectionTools.GetEnumeratorReset(enType.GetMethod("Reset"), enType);
        }
        public bool IsForeach
        {
            get;
        }
        public void Foreach ( object source, Action<ObjectBody> action )
        {
            IReflectionBody body = null;
            if ( this._IsObject )
            {
                body = ReflectionHepler.GetReflection(this._ElementType);
            }
            object em = this._Action(source);
            try
            {
                while ( this._Next(em) )
                {
                    object val = this._Current(em);
                    action(new ObjectBody(val, body));
                }
            }
            catch ( Exception e )
            {
                throw new ErrorException(e, "reflection.foreach.error", new Dictionary<string, string>
                {
                    {"Type",this._Type.FullName }
                });
            }
            finally
            {
                this._Reset(em);
            }
        }
        public bool TrueForAll ( object source, Func<object, bool> action )
        {
            object em = this._Action(source);
            try
            {
                while ( this._Next(em) )
                {
                    object val = this._Current(em);
                    if ( !action(val) )
                    {
                        this._Reset(em);
                        return false;
                    }
                }
                return true;
            }
            catch ( Exception e )
            {
                throw new ErrorException(e, "reflection.trueforall.error", new Dictionary<string, string>
                {
                    {"Type",this._Type.FullName }
                });
            }
            finally
            {
                this._Reset(em);
            }
        }

    }
}
