using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class BoolParse : IStringParse
    {
        public Type Type => PublicDataDic.BoolType;

        public dynamic Parse(string str, Type type)
        {
            return bool.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = bool.TryParse(str, out bool val);
            result = val;
            return ok;
        }
    }
}
