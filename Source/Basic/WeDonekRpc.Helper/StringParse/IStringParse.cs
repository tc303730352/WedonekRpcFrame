using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal interface IStringParse
    {
        Type Type { get; }
        bool TryParse(string str, Type type, out dynamic result);

        dynamic Parse(string str, Type type);
    }
}
