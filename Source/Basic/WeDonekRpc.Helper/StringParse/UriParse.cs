using System;

namespace WeDonekRpc.Helper.StringParse
{
    internal class UriParse : IStringParse
    {
        public Type Type => PublicDataDic.UriType;

        public dynamic Parse(string str, Type type)
        {
            return new Uri(str, UriKind.RelativeOrAbsolute);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out Uri val);
            result = val;
            return ok;
        }

    }
}
