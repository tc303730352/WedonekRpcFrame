using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class ULogParse : IStringParse
    {
        public Type Type => PublicDataDic.ULongType;

        public dynamic Parse(string str, Type type)
        {
            return ulong.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = ulong.TryParse(str, out ulong val);
            result = val;
            return ok;
        }
    }
}
