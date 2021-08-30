using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Param;
using SqlExecHelper.Where;

namespace SqlExecHelper
{

        public class InSqlWhere<T> : SqlBasicLinkWhere, ISqlTableWhere
        {
                private readonly T[] _Value = null;
                private readonly ISqlBasic _InSql = null;
                public InSqlWhere(string name, SqlDbType type, int size, T[] vals) : this(name, type, vals)
                {
                        this._Size = size;
                }
                public InSqlWhere(string name, SqlDbType type, T[] vals)
                {
                        this._Name = name;
                        this._DbType = type;
                        this._Value = vals;
                }
                public InSqlWhere(string name, T[] vals)
                {
                        this._Name = name;
                        this._DbType = SqlToolsHelper.GetSqlDbType(typeof(T));
                        this._Value = vals;
                }
                public InSqlWhere(string name, string table, T[] vals) : base(table)
                {
                        this._Name = name;
                        this._DbType = SqlToolsHelper.GetSqlDbType(typeof(T));
                        this._Value = vals;
                }
                public bool IsNot { get; set; }

                private readonly SqlDbType _DbType = SqlDbType.BigInt;

                private string _Name;

                private readonly int _Size = 0;



                public override void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {
                        if (this._Value.IsNull())
                        {
                                return;
                        }
                        if (this.IsAnd)
                        {
                                sql.Append("and ");
                        }
                        else
                        {
                                sql.Append("or ");
                        }
                        StringBuilder str = new StringBuilder();
                        this._Name = config.FormatName(this._Name);
                        this._Value.ForEach((a, i) =>
                        {
                                BasicParameter t = new BasicParameter(this._DbType, this._Size) { Value = a };
                                t.InitParam(config);
                                param.Add(t.GetParameter());
                                str.Append(",");
                                str.Append(t.ParamName);
                        });
                        str.Remove(0, 1);
                        sql.AppendFormat("{0} {2}in ({1}) ", this._Name, str.ToString(), this.IsNot ? "not " : string.Empty);
                }


        }
}
