using System;
using System.Text;

namespace WeDonekRpc.Helper.StringParse
{
    internal class ByteArrayParse : IStringParse
    {
        public Type Type => typeof(byte[]);

        public dynamic Parse(string str, Type type)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            result = Encoding.UTF8.GetBytes(str);
            return true;
        }
    }
}
