using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Config;
using SqlExecHelper.Query;
using SqlExecHelper.Where;

namespace SqlExecHelper
{
        public class InSqlWhere : SqlBasicLinkWhere, ISqlTableWhere
        {
                private readonly ISqlBasic _InSql = null;
                public InSqlWhere(string name, string tableName, string column, params ISqlWhere[] where)
                {
                        this._Name = name;
                        this._InSql = new SqlQuery(tableName, "c", SqlConfig.QueryLockType, column, where);
                }
                public InSqlWhere(string name, string tableName, params ISqlWhere[] where) : this(name, tableName, name, where)
                {
                }
                public bool IsNot { get; set; }
                private string _Name;

                public override void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {
                        if (this.IsAnd)
                        {
                                sql.Append("and ");
                        }
                        else
                        {
                                sql.Append("or ");
                        }
                        this._Name = config.FormatName(this._Name);
                        StringBuilder inSql = this._InSql.GenerateSql(out IDataParameter[] prs);
                        param.AddRange(prs);
                        sql.AppendFormat("{0} {2}in ({1}) ", this._Name, inSql.ToString(), this.IsNot ? "not " : string.Empty);
                }


        }
}
