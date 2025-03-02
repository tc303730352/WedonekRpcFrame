using System;

namespace WeDonekRpc.Helper.Reflection
{
    internal interface IGenericForeach
    {
        bool IsForeach { get; }
        void Foreach(object source, Action<ObjectBody> action);

        bool TrueForAll(object source, Func<object, bool> action);
    }
}