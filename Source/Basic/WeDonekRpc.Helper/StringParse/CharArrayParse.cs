using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class CharArrayParse :IStringParse
    {
        public Type Type => typeof(char[]);

        public dynamic Parse(string str, Type type)
        {
            return str.ToCharArray();
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            result = str.ToCharArray();
            return true;
        }
    }
}
