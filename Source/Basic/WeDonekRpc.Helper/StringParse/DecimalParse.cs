using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class DecimalParse : IStringParse
    {
        public Type Type => PublicDataDic.DecimalType;

        public dynamic Parse(string str, Type type)
        {
            return decimal.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = decimal.TryParse(str, out decimal val);
            result = val;
            return ok;
        }
    }
}
