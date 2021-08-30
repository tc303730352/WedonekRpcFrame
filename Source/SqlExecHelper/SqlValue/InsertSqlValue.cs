using System;
using System.Collections.Generic;
using System.Data;

using SqlExecHelper.Cache;
using SqlExecHelper.Param;

namespace SqlExecHelper.SqlValue
{
        public class InsertSqlValue : BasicParameter, IInsertSqlValue
        {
                private readonly string _Column = null;

                public string ColumnName => this._Column;

                public InsertSqlValue(BasicColumn column) : base(column.DbType)
                {
                        this._Column = column.Name;
                }
                public InsertSqlValue(string colName, SqlDbType type, int size) : base(type, size)
                {
                        this._Column = colName;
                }
                public InsertSqlValue(string colName, SqlDbType type) : base(type)
                {
                        this._Column = colName;
                }
                public InsertSqlValue(string colName, Type type) : base(SqlToolsHelper.GetSqlDbType(type))
                {
                        this._Column = colName;
                }
                public string GetValue(ISqlRunConfig config, List<IDataParameter> param)
                {
                        if (this.IsNull && this.Value == null)
                        {
                                return "null";
                        }
                        this.InitParam(config);
                        param.Add(this.GetParameter());
                        return this.ParamName;
                }
        }
}
