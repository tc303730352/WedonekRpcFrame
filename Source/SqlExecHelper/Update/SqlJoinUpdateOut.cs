using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;

namespace SqlExecHelper.Update
{
        internal class SqlJoinUpdateOut : SqlJoinUpdate
        {
                public SqlJoinUpdateOut(string table, string joinTable, RunParam param, RunParam joinParam, ClassStructure res, SqlEventPrefix prefix, ISqlSetColumn[] columns, params ISqlWhere[] where) : base(table, joinTable, param, joinParam, columns, where)
                {
                        this.ReturnCol = res.GetSetReturnColumn(prefix);
                }
                public SqlJoinUpdateOut(string table, string joinTable, RunParam param, RunParam joinParam, string column, SqlEventPrefix prefix, ISqlSetColumn[] columns, params ISqlWhere[] where) : base(table, joinTable, param, joinParam, columns, where)
                {
                        this.ReturnCol = new SqlUpdateColumn[]
                         {
                                new SqlUpdateColumn(column, prefix)
                         };
                }
                public SqlUpdateColumn[] ReturnCol
                {
                        get;
                }

                protected override void _InitOutput(StringBuilder sql)
                {
                        sql.Append(" output ");
                        this.ReturnCol.ForEach(a =>
                        {
                                sql.Append(a.ToString());
                        });
                        sql.Remove(sql.Length - 1, 1);
                }
        }
}
