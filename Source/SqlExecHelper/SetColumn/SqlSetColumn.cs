using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Param;

namespace SqlExecHelper.SetColumn
{
        public class SqlSetColumn : BasicParameter, ISqlSetColumn
        {
                public SqlSetColumn(BasicColumn column) : base(column.DbType)
                {
                        this._ColumnName = column.Name;
                }
                public SqlSetColumn(string colName, SqlDbType type) : base(type)
                {
                        this._ColumnName = colName;
                }
                public SqlSetColumn(string colName, SqlDbType type, int size) : base(type, size)
                {
                        this._ColumnName = colName;
                }
                public SqlSetColumn(string colName, SqlDbType type, SqlSetType setType) : base(type)
                {
                        this._ColumnName = colName;
                        this._SetType = setType;
                }
                public SqlSetColumn(string colName, SqlDbType type, int size, SqlSetType setType) : base(type, size)
                {
                        this._ColumnName = colName;
                        this._SetType = setType;
                }
                private readonly SqlSetType _SetType = SqlSetType.等于;
                private readonly string _ColumnName = null;
                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {
                        base.InitParam(config);
                        string col = config.FormatName(this._ColumnName);
                        param.Add(this.GetParameter());
                        if (this._SetType == SqlSetType.等于)
                        {
                                sql.AppendFormat("{0}={1},", col, this.ParamName);
                        }
                        else if (this._SetType == SqlSetType.递加)
                        {
                                sql.AppendFormat("{0}={0}+{1},", col, this.ParamName);
                        }
                        else
                        {
                                sql.AppendFormat("{0}={0}-{1},", col, this.ParamName);
                        }
                }

        }
}
