using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Interface;
using SqlExecHelper.Param;

namespace SqlExecHelper
{
        public class BasicSqlWhere : WhereSqlParameter, IQueryParameter
        {
                public BasicSqlWhere(string name, SqlDbType type, string symbol) : base(name, type)
                {
                        this.Symbol = symbol;
                }
                public BasicSqlWhere(string name, SqlDbType type, int size, string symbol) : base(name, type, size)
                {
                        this.Symbol = symbol;
                }
                public BasicSqlWhere(string name, SqlDbType type, object value, int size, string symbol) : base(name, type, value, size)
                {
                        this.Symbol = symbol;
                }
                /// <summary>
                /// 所属表
                /// </summary>
                public string Table
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否使用AND连接
                /// </summary>
                public bool IsAnd
                {
                        get;
                        set;
                } = true;

                /// <summary>
                /// 符号
                /// </summary>
                public string Symbol
                {
                        get;
                        protected set;
                }

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {
                        base.InitParam(config);
                        sql.Append(this.IsAnd ? "and " : "or ");
                        if (this.IsNull && this.Value == null)
                        {
                                sql.AppendFormat("{0} is null ", this.ColumnName);
                        }
                        else
                        {
                                param.Add(base.GetParameter());
                                sql.AppendFormat("{0} {1} {2} ", this.ColumnName, this.Symbol, this.ParamName);
                        }
                }

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, ISqlRunConfig other, List<IDataParameter> param)
                {
                        if (config.TableName == this.Table || this.Table == null)
                        {
                                this.GenerateSql(sql, config, param);
                        }
                        else
                        {
                                this.GenerateSql(sql, other, param);
                        }
                }
        }
}
