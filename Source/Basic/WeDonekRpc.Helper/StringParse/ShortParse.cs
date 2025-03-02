using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class ShortParse: IStringParse
    {
        public Type Type => PublicDataDic.ShortType;

        public dynamic Parse(string str, Type type)
        {
            return short.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = short.TryParse(str, out short val) ;
            result = val;
            return ok;
        }
    }
}
