using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Config;

namespace SqlExecHelper.Update
{
        internal class SqlJoinUpdate : ISqlBasic
        {
                private readonly string _Name;
                private readonly ISqlSetColumn[] _Column = null;
                private readonly ISqlRunConfig _MainConfig = null;
                private readonly string _JoinTable = null;
                private readonly ISqlRunConfig _JoinConfig = null;
                public ISqlWhere[] Where { get; }

                public ISqlRunConfig Config => this._MainConfig;

                public SqlJoinUpdate(string table, string joinTable, RunParam param, RunParam joinParam, ISqlSetColumn[] columns, params ISqlWhere[] where)
                {
                        this._JoinTable = SqlTools.GetTableName(joinTable, "a", joinParam);
                        this._JoinConfig = new SqlJoinConfig(joinTable, "a");
                        this._Name = SqlTools.GetTableName(table, param);
                        this._Column = columns;
                        this._MainConfig = new SqlJoinConfig(table, table);
                        this.Where = where;
                }
                protected virtual void _InitOutput(StringBuilder sql)
                {

                }
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(256);
                        sql.AppendFormat("update {0} set ", this._Name);
                        List<IDataParameter> list = new List<IDataParameter>();
                        SqlTools.InitSetColumn(sql, this._Column, this._MainConfig, this._JoinConfig, list);
                        this._InitOutput(sql);
                        sql.Append(" from ");
                        sql.Append(this._JoinTable);
                        if (this.Where != null && this.Where.Length > 0)
                        {
                                SqlTools.InitWhere(sql, this.Where, this._MainConfig, this._JoinConfig, list);
                        }
                        param = list.ToArray();
                        return sql;
                }
        }
}
