using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;
using SqlExecHelper.SqlValue;

namespace SqlExecHelper.Insert
{
        internal class SqlInsertOut : SqlInsert
        {
                public SqlInsertOut(IInsertSqlValue[] values, ISqlRunConfig config, ClassStructure result) : base(values, config)
                {
                        this.ReturnCol = result.GetBasicColumn();
                }
                public SqlInsertOut(IInsertSqlValue[] values, ISqlRunConfig config, string column) : base(values, config)
                {
                        this.ReturnCol = new BasicSqlColumn[] {
                                new BasicSqlColumn(column)
                        };
                }
                public SqlInsertOut(ClassStructure obj, string table, object val, ClassStructure result) : base(obj, table, val)
                {
                        this.ReturnCol = result.GetBasicColumn();
                }
                public SqlInsertOut(ClassStructure obj, string table, object val, string column) : base(obj, table, val)
                {
                        this.ReturnCol = new BasicSqlColumn[] {
                                new BasicSqlColumn(column)
                        };
                }
                public BasicSqlColumn[] ReturnCol
                {
                        get;
                }

                protected override string _GetOutput()
                {
                        StringBuilder str = new StringBuilder(" output ", 64);
                        this.ReturnCol.ForEach(a =>
                        {
                                if (a.AliasName != null)
                                {
                                        str.AppendFormat("inserted.{0} as {1},", a.Name, a.AliasName);
                                }
                                else
                                {
                                        str.AppendFormat("inserted.{0},", a.Name);
                                }
                        });
                        str.Remove(str.Length - 1, 1);
                        return str.ToString();
                }
        }
}
