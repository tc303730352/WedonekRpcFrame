using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class UIntParse : IStringParse
    {
        public Type Type => PublicDataDic.UIntType;

        public dynamic Parse(string str, Type type)
        {
            return uint.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = uint.TryParse(str, out uint val);
            result = val;
            return ok;
        }
    }
}
