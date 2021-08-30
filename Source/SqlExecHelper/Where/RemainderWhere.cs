using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Interface;
using SqlExecHelper.Param;

namespace SqlExecHelper
{
        /// <summary>
        /// 取余查询
        /// </summary>
        public class RemainderWhere : WhereSqlParameter, IQueryParameter
        {
                public RemainderWhere(string name, long val, bool isContain = true) : base(name, SqlDbType.BigInt, val)
                {
                        this._IsContain = isContain;
                }
                public RemainderWhere(string name, int val, bool isContain = true) : base(name, SqlDbType.Int, val)
                {
                        this._IsContain = isContain;
                }
                /// <summary>
                /// 所属表
                /// </summary>
                public string Table
                {
                        get;
                        set;
                }
                private readonly bool _IsContain = false;
                /// <summary>
                /// 是否使用AND连接
                /// </summary>
                public bool IsAnd
                {
                        get;
                        set;
                } = true;
                /// <summary>
                /// 是否是等号
                /// </summary>
                public bool IsEqual
                {
                        get;
                        set;
                } = false;

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {
                        base.InitParam(config);
                        param.Add(base.GetParameter());
                        sql.Append(this.IsAnd ? "and " : "or ");
                        if (this._IsContain)
                        {
                                sql.AppendFormat("({0} & {1}){2}{0} ", this.ParamName, this.ColumnName, this.IsEqual ? "=" : "!=");
                        }
                        else
                        {
                                sql.AppendFormat("({0} & {1}){2}{0} ", this.ColumnName, this.ParamName, this.IsEqual ? "=" : "!=");
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
