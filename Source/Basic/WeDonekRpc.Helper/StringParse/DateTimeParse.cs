using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class DateTimeParse : IStringParse
    {
        public Type Type => PublicDataDic.DateTimeType;

        public dynamic Parse(string str,Type type)
        {
            return DateTime.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = DateTime.TryParse(str, out DateTime val);
            result = val;
            return ok;
        }
    }
}
