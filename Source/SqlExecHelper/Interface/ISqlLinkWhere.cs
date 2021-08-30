using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper
{
        public interface ISqlLinkWhere : ISqlWhere
        {
                void GenerateSql(StringBuilder sql, ISqlRunConfig config, ISqlRunConfig other, List<IDataParameter> param);
        }
}