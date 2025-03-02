using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WeDonekRpc.HttpApiDoc.Collect
{
    internal class EnumCollect
    {
        private static readonly Dictionary<string, EnumShow> _EnumList = new Dictionary<string, EnumShow>();

        public static EnumShow GetEnumFormat(string name)
        {
            return _EnumList.TryGetValue(name, out EnumShow format) ? format : null;
        }
        internal static string LoadEnum(Type type, out int devVal)
        {
            string name = Tools.GetMD5(type.FullName).ToLower();
            if (_EnumList.TryGetValue(name, out EnumShow values))
            {
                devVal = values.Values.Min(a => a.Value);
                return name;
            }
            else
            {
                Array array = Enum.GetValues(type);
                List<EnumValue> vals = new List<EnumValue>();
                foreach (object i in array)
                {
                    vals.Add(new EnumValue
                    {
                        Name = i.ToString(),
                        Value = Convert.ToInt32(i)
                    });
                }
                devVal = vals.Min(a => a.Value);
                _EnumList.Add(name, new EnumShow
                {
                    EnumName = type.Name,
                    Values = vals.ToArray()
                });
                return name;
            }
        }
    }
}
