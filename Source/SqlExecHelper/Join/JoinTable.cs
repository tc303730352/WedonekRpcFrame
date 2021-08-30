using System.Text;

using SqlExecHelper.Config;
namespace SqlExecHelper.Join
{
        internal class JoinTable
        {
                public JoinTable(string table, string alias, RunParam param)
                {
                        this.TableName = table;
                        this._Name = SqlTools.GetTableName(table, alias, param);
                        this.Config = new SqlJoinConfig(table, alias);
                }
                public string TableName
                {
                        get;
                }
                private readonly string _Name = null;
                public ISqlRunConfig Config { get; }

                public void InitTable(StringBuilder sql)
                {
                        sql.Append(this._Name);
                }
        }
}
