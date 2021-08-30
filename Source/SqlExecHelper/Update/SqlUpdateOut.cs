using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Column;

namespace SqlExecHelper.Update
{
        internal class SqlUpdateOut : SqlUpdate
        {
                public SqlUpdateOut(string table, RunParam param, ISqlSetColumn[] columns, ClassStructure res, params ISqlWhere[] where) : base(table, param, columns, where)
                {
                        this.ReturnCol = res.GetSetReturnColumn();
                }
                public SqlUpdateOut(string table, RunParam param, ISqlSetColumn[] columns, ClassStructure res, SqlEventPrefix prefix, params ISqlWhere[] where) : base(table, param, columns, where)
                {
                        this.ReturnCol = res.GetSetReturnColumn(prefix);
                }
                public SqlUpdateOut(string table, RunParam param, ISqlSetColumn[] columns, string column, SqlEventPrefix prefix, params ISqlWhere[] where) : base(table, param, columns, where)
                {
                        this.ReturnCol = new SqlUpdateColumn[]
                       {
                                new SqlUpdateColumn(column, prefix)
                       };
                }
                public SqlUpdateOut(string table, RunParam param, ClassStructure obj, object data, ClassStructure res, params ISqlWhere[] where) : base(table, param, obj, data, where)
                {
                        this.ReturnCol = res.GetSetReturnColumn();
                }
                public SqlUpdateOut(string table, RunParam param, ClassStructure obj, object data, ClassStructure res, SqlEventPrefix prefix, params ISqlWhere[] where) : base(table, param, obj, data, where)
                {
                        this.ReturnCol = res.GetSetReturnColumn(prefix);
                }
                public SqlUpdateOut(string table, RunParam param, ClassStructure obj, object data, string column, SqlEventPrefix prefix, params ISqlWhere[] where) : base(table, param, obj, data, where)
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
