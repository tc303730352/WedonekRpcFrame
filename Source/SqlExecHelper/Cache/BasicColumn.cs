using System;
using System.Data;
using System.Reflection;

namespace SqlExecHelper.Cache
{
        public class BasicColumn
        {
                private static readonly Type _AttrType = typeof(SqlColumnType);
                private static readonly Type _Ignore = typeof(SqlIgnore);
                private static readonly Type _TableAttrType = typeof(SqlTable);
                public string Name
                {
                        get;
                        private set;
                }
                public SqlIgnoreType IgnoreType
                {
                        get;
                        set;
                }
                public string TableName
                {
                        get;
                        set;
                }
                public SqlDbType DbType
                {
                        get;
                        private set;
                }
                public string AliasName
                {
                        get;
                        private set;
                }
                public string ColumnFunc { get; private set; }
                public SqlEventPrefix SqlPrefix
                {
                        get;
                        private set;
                }
                private PropertyInfo _Pro = null;
                public static BasicColumn GetColumn(PropertyInfo pro)
                {
                        BasicColumn col = new BasicColumn
                        {
                                _Pro = pro,
                                Name = pro.Name
                        };
                        SqlTable table = (SqlTable)pro.GetCustomAttribute(_TableAttrType);
                        if (table != null)
                        {
                                col.TableName = table.TableName;
                        }
                        SqlIgnore ignore = (SqlIgnore)pro.GetCustomAttribute(_Ignore);
                        if (ignore != null)
                        {
                                col.IgnoreType = ignore.IgnoreType;
                        }
                        SqlColumnType attr = (SqlColumnType)pro.GetCustomAttribute(_AttrType);
                        if (attr == null)
                        {
                                col.DbType = SqlToolsHelper.GetSqlDbType(pro.PropertyType);
                        }
                        else
                        {
                                if (attr.DbType != null)
                                {
                                        col.DbType = (SqlDbType)attr.DbType;
                                }
                                if (attr.Name != null)
                                {
                                        col.Name = attr.Name;
                                        col.AliasName = pro.Name;
                                        col.SqlPrefix = attr.SqlPrefix;
                                        col.ColumnFunc = attr.ColumnFunc;
                                }
                                else
                                {
                                        col.SqlPrefix = SqlEventPrefix.deleted;
                                }
                        }
                        return col;
                }

                public object GetValue(object val)
                {
                        return this._Pro.GetValue(val);
                }
        }
}
