using System;
using System.Collections.Concurrent;

using SqlExecHelper.Column;

namespace SqlExecHelper.Cache
{
        internal class ClassStructureCache
        {
                private static readonly ConcurrentDictionary<string, ClassStructure> _Structure = new ConcurrentDictionary<string, ClassStructure>();

                public static ClassStructure GetStructure(Type type)
                {
                        if (_Structure.TryGetValue(type.FullName, out ClassStructure structure))
                        {
                                return structure;
                        }
                        structure = ClassStructure.GetStructure(type);
                        _Structure.TryAdd(type.FullName, structure);
                        return structure;
                }
                public static SqlColumn[] GetQueryColumn(Type type)
                {
                        ClassStructure obj = GetStructure(type);
                        return obj.GetQueryColumn();
                }
                public static BasicSqlColumn[] GetBasicColumn(Type type)
                {
                        ClassStructure obj = GetStructure(type);
                        return obj.GetBasicColumn();
                }
                public static SqlUpdateColumn[] GetReturnColumn(Type type)
                {
                        ClassStructure obj = GetStructure(type);
                        return obj.GetSetReturnColumn();
                }
                public static SqlUpdateColumn[] GetReturnColumn(Type type, SqlEventPrefix prefix)
                {
                        ClassStructure obj = GetStructure(type);
                        return obj.GetSetReturnColumn(prefix);
                }
        }
}
