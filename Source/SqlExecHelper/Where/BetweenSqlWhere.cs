using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Param;
using SqlExecHelper.Where;

namespace SqlExecHelper
{
        public class BetweenSqlWhere<T> : SqlBasicLinkWhere, ISqlTableWhere
        {
                private readonly string _Name;
                private readonly SqlDbType _DbType = SqlDbType.BigInt;
                private readonly int _Size = 0;
                private readonly T _Begin;
                private readonly T _End;
                private readonly string _BeginCol;
                private readonly string _EndCol;
                private readonly T _Value;
                private readonly bool _IsBack = false;
                public BetweenSqlWhere(string name, SqlDbType type, int size, T begin, T end) : this(name, type, begin, end)
                {
                        this._Size = size;
                }
                public BetweenSqlWhere(string name, SqlDbType type, T begin, T end)
                {
                        this._Name = name;
                        this._DbType = type;
                        this._Begin = begin;
                        this._End = end;
                }
                public BetweenSqlWhere(SqlDbType type, T data, string begin, string end)
                {
                        this._IsBack = true;
                        this._Value = data;
                        this._DbType = type;
                        this._BeginCol = begin;
                        this._EndCol = end;
                }
                public BetweenSqlWhere(SqlDbType type, int size, T data, string begin, string end) : this(type, data, begin, end)
                {
                        this._Size = size;
                }
                public bool IsNot { get; set; }

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
                        if (this._IsBack)
                        {
                                BasicParameter t = new BasicParameter(this._DbType, this._Size) { Value = _Value };
                                t.InitParam(config);
                                param.Add(t.GetParameter());
                                sql.AppendFormat("{0} {3}between {1} and {2} ", t.ParamName, config.FormatName(this._BeginCol), config.FormatName(this._EndCol), this.IsNot ? "not " : string.Empty);
                        }
                        else
                        {
                                string col = config.FormatName(this._Name);
                                BasicParameter one = new BasicParameter(this._DbType, this._Size) { Value = _Begin };
                                one.InitParam(config);
                                param.Add(one.GetParameter());
                                BasicParameter two = new BasicParameter(this._DbType, this._Size) { Value = _End };
                                two.InitParam(config);
                                param.Add(two.GetParameter());
                                sql.AppendFormat("{0} {3}between {1} and {2} ", col, one.ParamName, two.ParamName, this.IsNot ? "not " : string.Empty);
                        }
                }
        }
}
