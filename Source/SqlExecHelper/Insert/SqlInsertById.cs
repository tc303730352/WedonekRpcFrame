using System.Collections.Generic;
using System.Data;
using System.Text;

using SqlExecHelper.Cache;
using SqlExecHelper.Param;
using SqlExecHelper.SqlValue;

namespace SqlExecHelper.Insert
{
        internal class SqlInsertById : SqlInsert
        {
                public SqlInsertById(IInsertSqlValue[] values, ISqlRunConfig config, SqlDbType type) : base(values, config)
                {
                        this.IdType = type;
                }
                public SqlInsertById(IInsertSqlValue[] values, string table, SqlDbType type) : base(values, table)
                {
                        this.IdType = type;
                }
                public SqlInsertById(ClassStructure obj, string table, object val, SqlDbType type) : base(obj, table, val)
                {
                        this.IdType = type;
                }
                /// <summary>
                /// 返回的ID类型
                /// </summary>
                public SqlDbType IdType
                {
                        get;
                }
                protected override void _InitFoot(StringBuilder sql, List<IDataParameter> param)
                {
                        BasicParameter t = new BasicParameter(this.IdType) { Direction = ParameterDirection.Output };
                        t.InitParam(this.Config);
                        param.Add(t.GetParameter());
                        sql.AppendFormat(";set {0}=SCOPE_IDENTITY();", t.ParamName);
                }
        }
}
