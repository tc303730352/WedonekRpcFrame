using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Config;

namespace SqlExecHelper.Drop
{
        internal class SqlDrop : ISqlBasic
        {
                public SqlDrop(string table, RunParam param, params ISqlWhere[] where)
                {
                        this._Name = SqlTools.GetTableName(table, param);
                        this.TableName = table;
                        this.Where = where;
                        this.Config = new SqlBasicConfig(table);
                }
                private readonly string _Name = null;
                public ISqlRunConfig Config
                {
                        get;
                }
                /// <summary>
                /// 表明
                /// </summary>
                public string TableName
                {
                        get;
                }

                /// <summary>
                /// 查询参数
                /// </summary>
                public ISqlWhere[] Where
                {
                        get;
                }

                private IDataParameter[] _InitWhere(StringBuilder sql)
                {
                        if (this.Where != null && this.Where.Length > 0)
                        {
                                List<IDataParameter> param = new List<IDataParameter>();
                                SqlTools.InitWhere(sql, this.Where, this.Config, param);
                                return param.ToArray();
                        }
                        return null;
                }
                protected virtual void _InitOutput(StringBuilder sql)
                {

                }

                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        StringBuilder sql = new StringBuilder(128);
                        sql.AppendFormat("delete from {0}", this._Name);
                        this._InitOutput(sql);
                        param = this._InitWhere(sql);
                        return sql;
                }
        }
}
