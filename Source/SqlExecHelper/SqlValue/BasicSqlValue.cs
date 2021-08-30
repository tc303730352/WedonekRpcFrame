using System;
using System.Collections.Generic;
using System.Data;

using SqlExecHelper.Param;

namespace SqlExecHelper.Column
{
        public class BasicSqlValue : ISqlColumnVal
        {
                public SqlDbType DbType { get; }
                public BasicSqlValue(object val)
                {
                        this.Value = val;
                        this.DbType = SqlToolsHelper.GetSqlDbType(Type.GetTypeCode(val.GetType()));
                }
                public BasicSqlValue(object val, SqlDbType type)
                {
                        this.Value = val;
                        this.DbType = type;
                }

                public string GetValue(ISqlRunConfig config, List<IDataParameter> param)
                {
                        BasicParameter basic = new BasicParameter(this.DbType) { Value = Value };
                        basic.InitParam(config);
                        param.Add(basic.GetParameter());
                        return basic.ParamName;
                }
                public object Value
                {
                        get;
                        set;
                }
        }
}
