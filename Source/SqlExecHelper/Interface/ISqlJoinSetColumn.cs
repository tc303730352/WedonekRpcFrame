using System.Text;

namespace SqlExecHelper
{
        public interface ISqlJoinSetColumn : ISqlSetColumn
        {
                void GenerateSql(StringBuilder sql, ISqlRunConfig main, ISqlRunConfig join);
        }
}