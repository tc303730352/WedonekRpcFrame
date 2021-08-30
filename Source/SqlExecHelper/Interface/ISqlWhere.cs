using System.Collections.Generic;
using System.Data;
using System.Text;
namespace SqlExecHelper
{
        public interface ISqlWhere
        {
                bool IsAnd { get; set; }

                void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param);
        }
}