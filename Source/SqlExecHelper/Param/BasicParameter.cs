using System.Data;
using System.Data.SqlClient;

namespace SqlExecHelper.Param
{
        public class BasicParameter : ISqlExtendParameter
        {
                public BasicParameter(SqlDbType type)
                {
                        this.DbType = type;
                }
                public BasicParameter(object val)
                {
                        this.DbType = SqlToolsHelper.GetSqlDbType(val.GetType());
                        this.Value = val;
                }
                public BasicParameter(SqlDbType type, object val)
                {
                        this.DbType = type;
                        this.Value = val;
                }

                public BasicParameter(SqlDbType type, int size) : this(type)
                {
                        this.Size = size;
                }
                public BasicParameter(SqlDbType type, object value, int size) : this(type, size)
                {
                        this.Value = value;
                }
                public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

                public SqlDbType DbType { get; }


                public int Size { get; protected set; }

                public object Value { get; set; }
                public bool IsNull { get; set; }

                public string ParamName { get; private set; }

                public virtual void InitParam(ISqlRunConfig config)
                {
                        this.ParamName = config.GetParamName();
                        if (this.Direction == ParameterDirection.Input || this.Direction == ParameterDirection.InputOutput)
                        {
                                this.Value = this._FormatValue(this.Value);
                        }
                }
                protected virtual object _FormatValue(object val)
                {
                        if (this.Value != null || !this.IsNull)
                        {
                                return SqlToolsHelper.FormatValue(this.DbType, val);
                        }
                        return null;
                }
                public virtual IDataParameter GetParameter()
                {
                        return new SqlParameter(this.ParamName, this.DbType, this.Size) { Value = Value, Direction = Direction };
                }
        }
}
