using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeDonekRpc.Helper.StringParse
{
    internal class CharParse
    {
        public Type Type => PublicDataDic.CharType;

        public dynamic Parse(string str, Type type)
        {
            return char.Parse(str);
        }

        public bool TryParse(string str, Type type, out dynamic result)
        {
            bool ok = char.TryParse(str, out char val);
            result = val;
            return ok;
        }
    }
}
