using System;

namespace SqlExecHelper
{
        [AttributeUsage(AttributeTargets.Property, Inherited = false)]
        public class SqlTable : Attribute
        {
                public SqlTable(string tableName)
                {
                        this.TableName = tableName;
                }

                public string TableName { get; }
        }
}
