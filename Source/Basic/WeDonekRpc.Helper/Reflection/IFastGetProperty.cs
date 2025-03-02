using System;

namespace WeDonekRpc.Helper.Reflection
{
    public interface IFastGetProperty
    {
        string Name { get; }

        Type Type { get; }

        bool IsRead { get; }


        object GetValue (object source);
    }
}
