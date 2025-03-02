using System;
using System.Net;

namespace WeDonekRpc.Helper.StringParse
{
    internal class IPEndPointParse : IStringParse
    {
        public Type Type => PublicDataDic.IPEndPointType;

        public dynamic Parse(string str, Type type)
        {
            return IPAddress.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = IPEndPoint.TryParse(str, out IPEndPoint val);
            result = val;
            return ok;
        }
    }
}
