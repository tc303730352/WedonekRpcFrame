using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeDonekRpc.Helper.StringParse
{
    internal class EnumParse : IStringParse
    {
        public Type Type => PublicDataDic.EnumType;

        public dynamic Parse(string str, Type type)
        {
            return EnumHelper.Parse(type, str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = EnumHelper.TryParse(type, str, out object val);
            result = val;
            return ok;
        }

    }
}
