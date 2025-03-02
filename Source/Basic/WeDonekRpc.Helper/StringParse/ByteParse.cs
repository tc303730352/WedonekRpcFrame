using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class ByteParse : IStringParse
    {
        public Type Type => PublicDataDic.ByteType;

        public dynamic Parse(string str, Type type)
        {
            return byte.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = byte.TryParse(str, out byte val);
            result = val;
            return ok;
        }
    }
}
