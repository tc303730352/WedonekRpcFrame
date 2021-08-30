using System.Collections.Generic;
using System.Data;

namespace SqlExecHelper.Column
{
        public class SqlColumnVal : ISqlColumnVal
        {
                private readonly SqlColumn _Column = null;

                public SqlColumnVal(SqlColumn column)
                {
                        this._Column = column;
                }

                public object Value { get; set; }

                public string GetValue(ISqlRunConfig config, List<IDataParameter> param)
                {
                        return config.FormatColumn(this._Column);
                }
        }
}
