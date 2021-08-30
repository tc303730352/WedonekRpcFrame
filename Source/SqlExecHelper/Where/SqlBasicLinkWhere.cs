using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper.Where
{
        public class SqlBasicLinkWhere : ISqlLinkWhere
        {
                public SqlBasicLinkWhere()
                {

                }
                public SqlBasicLinkWhere(string table)
                {
                        this.Table = table;
                }
                public string Table { get; set; }
                public bool IsAnd { get; set; } = true;

                public void GenerateSql(StringBuilder sql, ISqlRunConfig config, ISqlRunConfig other, List<IDataParameter> param)
                {
                        if (config.TableName == this.Table || this.Table == null)
                        {
                                this.GenerateSql(sql, config, param);
                        }
                        else
                        {
                                this.GenerateSql(sql, other, param);
                        }
                }

                public virtual void GenerateSql(StringBuilder sql, ISqlRunConfig config, List<IDataParameter> param)
                {

                }
        }
}
