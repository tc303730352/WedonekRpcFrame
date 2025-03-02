using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class IntParse: IStringParse
    {
        public Type Type => PublicDataDic.IntType;

        public dynamic Parse(string str,Type type)
        {
            return int.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = int.TryParse(str, out int val);
            result = val;
            return ok;
        }
    }
}
