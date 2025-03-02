using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class UShortParse : IStringParse
    {
        public Type Type => PublicDataDic.UShortType;

        public dynamic Parse(string str, Type type)
        {
            return ushort.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = ushort.TryParse(str, out ushort val);
            result = val;
            return ok;
        }
    }
}
