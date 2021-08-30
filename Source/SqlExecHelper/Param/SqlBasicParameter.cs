using System.Data;
using System.Data.SqlClient;
namespace SqlExecHelper
{
        public class SqlBasicParameter
        {
                public SqlBasicParameter(string name, SqlDbType type)
                {
                        this.Name = name[0] == '@' ? name : string.Concat("@", name);
                        this.DbType = type;
                }

                public SqlBasicParameter(string name, SqlDbType type, int size) : this(name, type)
                {
                        this.Size = size;
                }
                public SqlBasicParameter(string name, SqlDbType type, object value, int size) : this(name, type, size)
                {
                        this.Value = value;
                }

                public string Name { get; }
                public SqlDbType DbType { get; }
                public int Size { get; }
                public object Value { get; set; }
                public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

                internal IDataParameter GetParameter()
                {
                        if (this.Direction == ParameterDirection.Input || this.Direction == ParameterDirection.InputOutput)
                        {
                                object val = SqlToolsHelper.FormatValue(this.DbType, this.Value);
                                return new SqlParameter(this.Name, this.DbType, this.Size) { Value = val, Direction = Direction };
                        }
                        return new SqlParameter(this.Name, this.DbType, this.Size) { Direction = Direction };
                }
        }
}
