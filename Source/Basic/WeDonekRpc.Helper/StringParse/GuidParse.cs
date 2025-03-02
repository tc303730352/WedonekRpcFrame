using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class GuidParse : IStringParse
    {
        public Type Type => PublicDataDic.GuidType;

        public dynamic Parse(string str, Type type)
        {
            return Guid.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = Guid.TryParse(str, out Guid val);
            result = val;
            return ok;
        }
    }
}
