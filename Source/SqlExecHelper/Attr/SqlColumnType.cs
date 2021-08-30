using System;
using System.Data;

namespace SqlExecHelper
{
        [AttributeUsage(AttributeTargets.Property, Inherited = false)]
        public class SqlColumnType : Attribute
        {
                private static readonly int _VarcharMaxLen = 8000;

                public string Name
                {
                        get;
                }
                public SqlEventPrefix SqlPrefix
                {
                        get;
                }
                public SqlDbType? DbType { get; }

                public string ColumnFunc { get; }
                public SqlColumnType(SqlDbType dbType)
                {
                        this.DbType = dbType;
                }
                public SqlColumnType(string name)
                {
                        this.Name = name;
                }
                public SqlColumnType(string name, SqlFuncType type)
                {
                        this.Name = name;
                        this.ColumnFunc = SqlToolsHelper.GetSqlFunc(type); ;
                }
                public SqlColumnType(string name, string func)
                {
                        this.Name = name;
                        this.ColumnFunc = func;
                }
                public SqlColumnType(string name, SqlDbType dbType)
                {
                        this.Name = name;
                        this.DbType = dbType;
                }
                public SqlColumnType(string name, SqlEventPrefix prefix)
                {
                        this.SqlPrefix = prefix;
                        this.Name = name;
                }
                public SqlColumnType(long size)
                {
                        this.DbType = size > _VarcharMaxLen ? SqlDbType.NText : SqlDbType.NVarChar;
                }
        }
}
