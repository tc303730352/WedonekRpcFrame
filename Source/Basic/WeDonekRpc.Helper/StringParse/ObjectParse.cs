using System;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper.StringParse
{
    internal class ObjectParse : IStringParse
    {
        public Type Type => typeof(object);

        public dynamic Parse (string str, Type type)
        {
            return JsonTools.Json(str, type);
        }

        public bool TryParse (string str, Type type, out dynamic result)
        {
            result = JsonTools.Json(str, type);
            return true;
        }
    }
}
