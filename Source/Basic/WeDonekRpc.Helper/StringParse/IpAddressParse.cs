using System;
using System.Net;

namespace WeDonekRpc.Helper.StringParse
{
    internal class IpAddressParse : IStringParse
    {
        public Type Type => PublicDataDic.IPAddressType;

        public dynamic Parse(string str,Type type)
        {
            return IPAddress.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = IPAddress.TryParse(str, out IPAddress val);
            result = val;
            return ok;
        }
    }
}
