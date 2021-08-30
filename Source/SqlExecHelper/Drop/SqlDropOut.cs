using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;

namespace SqlExecHelper.Drop
{
        internal class SqlDropOut : SqlDrop
        {
                public SqlDropOut(string table, RunParam param, ClassStructure structure, params ISqlWhere[] where) : base(table, param, where)
                {
                        this.ReturnCol = structure.GetBasicColumn();
                }
                public SqlDropOut(string table, RunParam param, string column, params ISqlWhere[] where) : base(table, param, where)
                {
                        this.ReturnCol = new BasicSqlColumn[]
                        {
                                new BasicSqlColumn(column)
                        };
                }
                public BasicSqlColumn[] ReturnCol
                {
                        get;
                }
                protected override void _InitOutput(StringBuilder sql)
                {
                        sql.Append(" output ");
                        this.ReturnCol.ForEach(a =>
                        {
                                if (a.AliasName != null)
                                {
                                        sql.AppendFormat("deleted.{0} as {1},", a.Name, a.AliasName);
                                }
                                else
                                {
                                        sql.AppendFormat("deleted.{0},", a.Name);
                                }
                        });
                        sql.Remove(sql.Length - 1, 1);
                }

        }
}
